using Boo.Lang;
using Editor;
using UnityEditor;
using UnityEngine;

public class FSMEditor : EditorWindow
{
    private List<Node> _nodes;
    private List<Connection> _connections;
    
    private GUIStyle _nodeStyle;
    private GUIStyle _selectedNodeStyle;
    private GUIStyle _inPointStyle;
    private GUIStyle _outPointStyle;
    private ConnectionPoint _selectedInPoint;
    private ConnectionPoint _selectedOutPoint;
    private Vector2 _drag;
    private Vector2 _offset;
    
    [MenuItem("Window/FSMEditor %F12")]
    private static void OpenWindow()
    {
        FSMEditor window = GetWindow<FSMEditor>();
        window.titleContent = new GUIContent("FSM Editor", "A visual editor for creating Finite State Machines");
    }

    private void OnEnable()
    {
        _nodeStyle = new GUIStyle();
        _nodeStyle.normal.background = (Texture2D) EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png");
        _nodeStyle.border = new RectOffset(12,12,12,12);
        
        _selectedNodeStyle = new GUIStyle();
        _selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        _selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        _inPointStyle = new GUIStyle();
        _inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        _inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        _inPointStyle.border = new RectOffset(4, 4, 12, 12);
 
        _outPointStyle = new GUIStyle();
        _outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        _outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        _outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);
        
        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);
        
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        
        if(GUI.changed)
            Repaint();
    }

    private void DrawGrid(int gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);
 
        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
 
        _offset += _drag * 0.5f;
        Vector3 newOffset = new Vector3(_offset.x % gridSpacing, _offset.y % gridSpacing, 0);
 
        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }
 
        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }
 
        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawConnectionLine(Event e)
    {
        if (_selectedInPoint != null && _selectedOutPoint == null)
        {
            Handles.DrawBezier(
                _selectedInPoint.rect.center,
                e.mousePosition,
                _selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
    
            GUI.changed = true;
        }
    
        if (_selectedOutPoint != null && _selectedInPoint == null)
        {
            Handles.DrawBezier(
                _selectedOutPoint.rect.center,
                e.mousePosition,
                _selectedOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );
    
            GUI.changed = true;
        }   
    }

    private void DrawConnections()
    {
        if (_connections != null)
        {
            for (int i = 0; i < _connections.Count; i++)
            {
                _connections[i].Draw();
            }
        }
    }

    private void ProcessNodeEvents(Event current)
    {
        if (_nodes != null)
        {
            for (int i = _nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = _nodes[i].ProcessEvents(current);

                if (guiChanged)
                    GUI.changed = true;
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        _drag = Vector2.zero;
        switch (e.type)
        {  
                case EventType.MouseDown:
                    if(e.button == 0) ClearConnectionSelection();
                    
                    if (e.button == 1) ProcessContextMenu(e.mousePosition);
                    break;
                
                case EventType.MouseDrag:
                    if (e.button == 0) OnDrag(e.delta);
                    break;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        _drag = delta;

        if (_nodes != null)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }
    

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("New State Node", "Add a new State Node"), false, () => OnClickAddNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition)
    {
        if(_nodes == null)
            _nodes = new List<Node>();

        _nodes.Add(new Node(mousePosition, 200, 50, _nodeStyle, _inPointStyle, _outPointStyle,_selectedNodeStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    private void DrawNodes()
    {
        if (_nodes != null)
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                _nodes[i].Draw();
            }
        }
    }
    
    private void OnClickRemoveNode(Node node)
    {
        if (_connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();
 
            for (int i = 0; i < _connections.Count; i++)
            {
                if (_connections[i].inPoint == node.inPoint || _connections[i].outPoint == node.outPoint)
                {
                    connectionsToRemove.Add(_connections[i]);
                }
            }
 
            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                _connections.Remove(connectionsToRemove[i]);
            }
 
            connectionsToRemove = null;
        }
 
        _nodes.Remove(node);
    }
    
    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        _selectedInPoint = inPoint;
 
        if (_selectedOutPoint != null)
        {
            if (_selectedOutPoint.node != _selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection(); 
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
    
    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        _selectedOutPoint = outPoint;
 
        if (_selectedInPoint != null)
        {
            if (_selectedOutPoint.node != _selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }
 
    private void OnClickRemoveConnection(Connection connection)
    {
        _connections.Remove(connection);
    }
 
    private void CreateConnection()
    {
        if (_connections == null)
        {
            _connections = new List<Connection>();
        }
 
        _connections.Add(new Connection(_selectedInPoint, _selectedOutPoint, OnClickRemoveConnection));
    }
 
    private void ClearConnectionSelection()
    {
        _selectedInPoint = null;
        _selectedOutPoint = null;
    }
}
   