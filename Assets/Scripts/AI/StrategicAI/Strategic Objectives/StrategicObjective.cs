using System.Collections.Generic;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace StrategicAI
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
    
    [System.Serializable]
    public class StrategicObjective : MonoBehaviour
    {
        [SerializeField] private ObjectiveType _objectiveType;
        
        public ObjectiveType ObjectiveType => _objectiveType;
    }
}