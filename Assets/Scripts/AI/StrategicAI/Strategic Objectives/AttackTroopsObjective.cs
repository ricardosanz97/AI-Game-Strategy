using System.Collections.Generic;
using InfluenceMap;
using StrategicAI;
using UnityEngine;

namespace StrategicAI
{
    public class AttackTroopsObjective : StrategicObjective
    {
        public override Entity DecideBasedOnInfluenceData(AbstractNPCBrain brain, List<Node> influenceData,
            Entity[] playerControlledEntites)
        {
            throw new System.NotImplementedException();
        }
    }
}