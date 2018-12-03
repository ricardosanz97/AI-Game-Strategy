using System.Collections.Generic;
using System.IO;
using System.Linq;
using Editor;
using FSMSO;
using UnityEditor;
using UnityEngine;
using Zenject;
using Array = System.Array;

public class FSMEditor : EditorWindow
{
    #region NodeVariables
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
    #endregion

    private Dictionary<string,string> foundAssets = null;
    private BrainConfiguration currentConfiguration;
    private string stateName = "No name";
    
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

    private void OnRemovedNode(Node node)
    {
        var states = currentConfiguration.states.ToList();
        states.Remove(node.FsmState);
        currentConfiguration.states = states.ToArray();
        node.OnRemoveNode -= OnRemovedNode;
    }

    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        if (GUILayout.Button("Find Assets"))
        {
            foundAssets = FindAssetsNameByType<FSMSO.BrainConfiguration>();
        }

        stateName = EditorGUILayout.TextField("State Name", stateName);
        
        DrawFoundFsmBrains();
        
        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);
        
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);
        
        if(GUI.changed)
            Repaint();
    }

    private void DrawFoundFsmBrains()
    {
        if (foundAssets == null)
            return;

        string[] assetsToDisplay = foundAssets.Keys.ToArray();
        int selected = EditorGUILayout.Popup("FSM Brains", 0, assetsToDisplay, EditorStyles.toolbarPopup);

        currentConfiguration =
            AssetDatabase.LoadAssetAtPath<BrainConfiguration>(foundAssets[assetsToDisplay[selected]]);
    }

    private Dictionary<string,string> FindAssetsNameByType<T>()
    {
        string[] guiDsFound = AssetDatabase.FindAssets("t:" + typeof(T));
        Dictionary<string,string> assets = new Dictionary<string,string>();
        
        if (guiDsFound != null)
        {
            foreach (var guid in guiDsFound)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string name = Path.GetFileName(path);
                assets[name] = path;
                Debug.Log(assets[name]);
            }
        }

        return assets;
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
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }
                    
                    if (e.button == 1) 
                        ProcessContextMenu(e.mousePosition);
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
        genericMenu.AddItem(new GUIContent("New State Node", "Add a new State Node"), false, () => OnClickAddNode(mousePosition,stateName));
        genericMenu.ShowAsContext();
    }

    private void OnClickAddNode(Vector2 mousePosition, string stateName)
    {
        if(_nodes == null)
            _nodes = new List<Node>();
        
        Node node = new Node(mousePosition, 200, 50, _nodeStyle, _inPointStyle, _outPointStyle,_selectedNodeStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,stateName);
        _nodes.Add(node);
        
        var assetsFound = AssetDatabase.FindAssets(stateName);

        if (assetsFound.Length > 0)
        {
            node.FsmState = AssetDatabase.LoadAssetAtPath<FSMSO.State>(AssetDatabase.GUIDToAssetPath(assetsFound[0]));
        }
        else
        {
            node.FsmState = ScriptableObject.CreateInstance<FSMSO.State>();
            AssetDatabase.CreateAsset(node.FsmState, "Assets/" + stateName + ".asset");
            AssetDatabase.SaveAssets();
        }

        node.OnRemoveNode += OnRemovedNode;
        
        var states = currentConfiguration.states.ToList();
        states.Add(node.FsmState);
        currentConfiguration.states = states.ToArray();
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
            Boo.Lang.List<Connection> connectionsToRemove = new Boo.Lang.List<Connection>();
 
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
   