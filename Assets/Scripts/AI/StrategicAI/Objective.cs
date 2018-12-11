using UnityEngine;

namespace AI.StrategicAI
{
    [System.Serializable]
    public class Objective
    {
        [System.Serializable]
        public enum ObjectiveType
        {
            Base,
            Troop,
            Entity,
            Cell
        }
        
        [SerializeField]private string description;
        [SerializeField]private ObjectiveType objectiveType;

        public Objective(string descriptio, ObjectiveType type)
        {
            this.description = description;
            this.objectiveType = this.objectiveType;
        }
    }
}