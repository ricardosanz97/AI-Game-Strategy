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
            if (analyzedNPC is Troop)
            {
                Debug.Log("generamos posibles nodos a mover. ");
                Entity closestCore = GetClosestEntityInCollection(analyzedNPC, coreEntities);
                analyzedNPC.GetComponent<Troop>().GetCellsWithEnemiesInRange();
                if (analyzedNPC.GetComponent<Troop>().possibleAttacks.Contains(closestCore.cell.PNode) && closestCore.entityType == ENTITY.Core){
                    return closestCore;
                }
                else
                {
                    return null;
                }
            }
                  
            //si hay null no pasa nada, se gestiona luego
            return null;
        }
    }
}