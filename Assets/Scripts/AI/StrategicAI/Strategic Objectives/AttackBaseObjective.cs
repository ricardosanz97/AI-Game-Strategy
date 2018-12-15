using System;
using System.Collections.Generic;
using InfluenceMap;
using UnityEngine;

namespace StrategicAI
{
    public class AttackBaseObjective : StrategicObjective
    {
        public override Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNPC, List<Node> influenceData,
            Entity[] playerControlledEntites, LevelController levelController)
        {
            Entity[] coreEntities = levelController.playerCoreEntities.ToArray();

            //Cuando se lanze este método, todas las unidades solo podran atacar al core.
            if (analyzedNPC.entityType == ENTITY.Launcher || analyzedNPC.entityType == ENTITY.Prisioner || analyzedNPC.entityType == ENTITY.Tank)
            {
                return GetClosestEntityInCollection(analyzedNPC, coreEntities);
            }
                  
            //si hay null no pasa nada, se gestiona luego
            return null;
        }
    }
}