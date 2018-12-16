using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using InfluenceMap;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingGridDebug : MonoBehaviour
{   
    private PathfindingGrid _grid;

    private void Start()
    {
        _grid = GetComponent<PathfindingGrid>();
    }

    private void OnGUI()
    {
        #if UNITY_EDITOR
        if (GUILayout.Button("CreateGrid"))
        {
            _grid.UpdateGrid(null);
        }
       
        #endif
    }
}
