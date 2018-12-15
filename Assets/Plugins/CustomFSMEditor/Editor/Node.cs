using System;
using DG.DOTweenEditor.UI;
using UnityEditor;
using UnityEngine;
using UnityScript.Macros;
using Zenject;

namespace Editor
{
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        
        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        public event Action<Node> OnRemoveNode;
        private Rect labelRect;
        private string labelName;
        
        #region FSMVariables
        public FSMSO.State FsmState;
        #endregion
 
        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
            GUIStyle selectedStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this,ConnectionPointType.In,inPointStyle,OnClickInPoint);
            outPoint = new ConnectionPoint(this,ConnectionPointType.Out,outPointStyle,OnClickOutPoint);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;
            labelRect = new Rect(rect.x + rect.width/2 - 25, rect.y + rect.height/2 - 12.5f, 50, 25);
            labelName = "default";

        }
        
        public Node(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
            GUIStyle selectedStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode,string labelName)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this,ConnectionPointType.In,inPointStyle,OnClickInPoint);
            outPoint = new ConnectionPoint(this,ConnectionPointType.Out,outPointStyle,OnClickOutPoint);
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
            OnRemoveNode = OnClickRemoveNode;
            labelRect = new Rect(rect.x + rect.width/2 - 25, rect.y + rect.height/2 - 12.5f, 50, 25);
            this.labelName = labelName;

        }
 
        public void Drag(Vector2 delta)
        {
            rect.position += delta;
            labelRect.position += delta;
        }
 
        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, title, style);
            EditorGUI.LabelField(labelRect,labelName);
        }
 
        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                            ShowNodeInspector();
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }

                    break;
                
                case EventType.KeyUp:
                    if (e.keyCode == KeyCode.Delete && isSelected)
                    {
                        OnClickRemoveNode();
                        e.Use();
                    }

                    break;
                
                case EventType.MouseUp:
                    isDragged = false;
                    break;
                
                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }

                    break;
                    
            }

            return false;
        }

        private void ShowNodeInspector()
        {
            var h = AssetDatabase.FindAssets(labelName);
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<FSMSO.State>(AssetDatabase.GUIDToAssetPath(h[0]));
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }
        
        private void OnClickRemoveNode()
        {
            OnRemoveNode?.Invoke(this);
        }

    }
    
}