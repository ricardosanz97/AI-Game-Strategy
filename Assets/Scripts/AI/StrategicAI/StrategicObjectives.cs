using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace AI.StrategicAI
{
    [System.Serializable]
    public class TasksDictionary : SerializableDictionaryBase<string, int>
    {
    }
    

    [CreateAssetMenu(menuName = "AI/StrategicObjectives", fileName = "AIObjectives", order = 0)]
    public class StrategicObjectives : ScriptableObject
    {
        public TasksDictionary TasksDictionary = new TasksDictionary();
    }
}