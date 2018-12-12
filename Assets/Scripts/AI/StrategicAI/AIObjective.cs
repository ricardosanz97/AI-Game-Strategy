using System;
using UnityEngine;

namespace StrategicAI
{
    [System.Serializable]
    public class AIObjective : IComparable<AIObjective>
    {
        [System.Serializable]
        public enum ObjectiveType
        {
            AttackBase,
            DefendBase,
            AttackDefenses,
            AttackTroops,
            UpgradeDefenses,
            ProtectTroops
        }
        
        public string description;
        public ObjectiveType objectiveType;
        
        [Tooltip("The smaller the more important")]
        public int priority;

        public AIObjective(string descriptio, ObjectiveType type)
        {
            this.description = description;
            this.objectiveType = this.objectiveType;
        }

        public int CompareTo(AIObjective other)
        {
            return this.priority.CompareTo(other.priority);
        }
    }
}