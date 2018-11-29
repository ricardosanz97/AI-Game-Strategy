using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Test : MonoBehaviour
{
	[Inject]private GameManager _gameManager;
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log(_gameManager.TestInt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
