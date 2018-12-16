using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class SelectSpawnables : EditorWindow {

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/Select Spawnables")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		SelectSpawnables window = (SelectSpawnables)EditorWindow.GetWindow(typeof(SelectSpawnables));
		window.Show();
	}

	private void OnGUI()
	{
		if(GUILayout.Button("Select ALL Spawnables"))
		{
			SpawnableCell[] spawnableCells = FindObjectsOfType<SpawnableCell>();

			List<GameObject>  selection = new List<GameObject>();
			
			foreach (var cell in spawnableCells)
			{
				if(cell.GetComponent<CellBehaviour>())
				{
					Debug.Log(cell.GetComponent<CellBehaviour>().owner);
					selection.Add(cell.gameObject);		
				}
			}
			
			Selection.objects = selection.ToArray();
		}
	}
}
