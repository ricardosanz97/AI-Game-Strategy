using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingGridDebug : MonoBehaviour
{
    [Range(0.3f,0.9f)]
    public float cubeSeparation = 0.4f;
    
    private PathfindingGrid _grid;

    private void Start()
    {
        _grid = GetComponent<PathfindingGrid>();
    }

    private void OnGUI()
    {
        #if UNITY_EDITOR
        if (GUILayout.Button("Generate Grid"))
        {
            _grid.CreateGrid();
        }
        #endif
    }
}
