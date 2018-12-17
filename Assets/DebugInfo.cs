using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using InfluenceMap;
using UnityEngine;

public class DebugInfo : MonoBehaviour
{
	
	private InfluenceMapComponent _influenceMapComponent;
	private PathfindingGrid _pathfindingGrid;
	
	private bool activated = true;	
	
	// Use this for initialization
	private void Awake()
	{
		_influenceMapComponent = GetComponent<InfluenceMapComponent>();
		_pathfindingGrid = FindObjectOfType<PathfindingGrid>();
	}
	
	private void OnGUI()
	{
		activated = GUILayout.Toggle(activated,"Show Debug Info");
		
		if(GUI.changed)
		{
			_influenceMapComponent.influenceGrid._renderGroundGrid = activated;
			_influenceMapComponent.influenceGrid.UpdateMap();
			_pathfindingGrid.ChangeNodeDisplayMode(activated);
		}
		
	}
}



