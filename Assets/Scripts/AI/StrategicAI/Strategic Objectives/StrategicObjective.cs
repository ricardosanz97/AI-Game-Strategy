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
    public class StrategicObjective : MonoBehaviour
    {
        [SerializeField] private ObjectiveType _objectiveType;
        [SerializeField] private int _sampleRadius;
        
        public ObjectiveType ObjectiveType => _objectiveType;
        public int SampleRadius => _sampleRadius;

        public void DecideBasedOnInfluenceData(List<Node> influenceData)
        {
            throw new System.NotImplementedException();
        }
    }
}