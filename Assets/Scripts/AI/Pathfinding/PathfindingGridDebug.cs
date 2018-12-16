using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using InfluenceMap;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingGridDebug : MonoBehaviour
{   
    private PathfindingGrid _grid;
    private InfluenceMapComponent _influenceMapComponent;

    private void Start()
    {
        _grid = GetComponent<PathfindingGrid>();
        _influenceMapComponent = FindObjectOfType<InfluenceMapComponent>();
    }

    private void OnGUI()
    {
        #if UNITY_EDITOR
        if (GUILayout.Button("CreateGrid"))
        {
            _grid.UpdateGrid(null);
        }
        
        bool value = GUILayout.Toggle(false,"Activate Influence Map");
        _influenceMapComponent.influenceGrid._renderGroundGrid = value;
       
        #endif
    }
}
