using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using InfluenceMap;
using ModestTree;
using UnityEngine;
using UnityEngine.UI;
using Assert = UnityEngine.Assertions.Assert;

public class DebugInfo : MonoBehaviour
{
	
	private InfluenceMapComponent _influenceMapComponent;
	private PathfindingGrid _pathfindingGrid;
	private LevelController _levelController;
	private RawImage _influenceTexture;
	
	private bool activated = true;	
	
	// Use this for initialization
	private void Awake()
	{
		_influenceMapComponent = FindObjectOfType<InfluenceMapComponent>();
		_levelController = FindObjectOfType<LevelController>();
		_pathfindingGrid = FindObjectOfType<PathfindingGrid>();
		_influenceTexture = GameObject.FindGameObjectWithTag("InfluenceTexture").GetComponent<RawImage>();
		
		Assert.IsNotNull(_influenceTexture);
		Assert.IsNotNull(_influenceMapComponent);
		Assert.IsNotNull(_levelController);
		Assert.IsNotNull(_pathfindingGrid);	
	}
	
	private void OnGUI()
	{
		activated = GUILayout.Toggle(activated,"Show Debug Info");
		
		if(GUI.changed)
		{
			_influenceMapComponent.influenceGrid._renderGroundGrid = activated;
			_influenceMapComponent.influenceGrid.UpdateMap();
			_pathfindingGrid.ChangeNodeDisplayMode(activated);

			foreach (var entity in _levelController.TotalEntities)
			{
				entity.GetComponent<AbstractNPCBrain>().currentStateDebug.enabled = activated;
			}
			
			_influenceTexture.enabled = activated;
		}
		
	}
}



