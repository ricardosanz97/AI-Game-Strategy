using System.Collections.Generic;
using InfluenceMap;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace StrategicAI
{        
    [System.Serializable]
    public abstract class StrategicObjective : MonoBehaviour
    {
        [SerializeField] private int _sampleRadius;
        public int SampleRadius => _sampleRadius;

        public abstract Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNpc, List<Node> influenceData,
            Entity[] playerControlledEntites);
    }
}