using System.Collections;
using System.Collections.Generic;
using AI.StrategicAI;
using UnityEngine;

[System.Serializable]
public class AiTask
{
	public int Priority;
	public string Description;
	public HighLevelAI.IAPersonality IaPersonality;
	
	private Entity Doer;
	private Entity Objective;

	public void Assign(Entity doer)
	{
		Debug.Log($"Task with priority: {Priority}assigned to: {Doer.name}");
		Doer = doer;
	}

	public bool IsAssigned()
	{
		return Doer != null;
	}
}
