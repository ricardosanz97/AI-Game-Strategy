using System;
using System.Collections.Generic;
using InfluenceMap;
using UnityEngine;

namespace StrategicAI
{
    public class AttackBaseObjective : StrategicObjective
    {
        public override Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNPC, List<Node> influenceData,
            Entity[] playerControlledEntites)
        {
            Entity coreEntity = playerControlledEntites[0];

            //Cuando se lanze este método, todas las unidades solo podran atacar al core.

            if (analyzedNPC.entityType == ENTITY.Launcher || analyzedNPC.entityType == ENTITY.Prisioner || analyzedNPC.entityType == ENTITY.Tank)
            {
                foreach (Entity e in playerControlledEntites)
                {
                    if (e.entityType == ENTITY.Core)
                    {
                        coreEntity = e;
                    }
                }
                return coreEntity;
            }
                  
            //solo para que no haya nulls
            return playerControlledEntites[0];
        }
    }
}