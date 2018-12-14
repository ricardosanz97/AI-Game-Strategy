using System.Collections.Generic;
using InfluenceMap;
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
    public abstract class StrategicObjective : MonoBehaviour
    {
        [SerializeField] private ObjectiveType _objectiveType;
        [SerializeField] private int _sampleRadius;
        
        public ObjectiveType ObjectiveType => _objectiveType;
        public int SampleRadius => _sampleRadius;

        public abstract Entity[] DecideBasedOnInfluenceData(List<Node> influenceData);
    }
}