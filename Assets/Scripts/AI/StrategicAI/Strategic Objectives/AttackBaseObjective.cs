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
            if (analyzedNPC.npc == TROOP.Launcher)
            {
                float coreInfluenceSum = 0f;
                float prisionerSum = 0f;
            
                foreach (var node in influenceData)
                {
                    if (node.HasInfluenceOfType(InfluenceType.Core))
                        coreInfluenceSum += node.GetInfluenceOfType(InfluenceType.Core).Value;
                    else if (node.HasInfluenceOfType(InfluenceType.Prisioner))
                        prisionerSum += node.GetInfluenceOfType(InfluenceType.Prisioner).Value;
                }

                if (coreInfluenceSum < prisionerSum) // entonces vamos a por el prisioner
                {
                    //todo encontrar la referencia al prisioner
                    //parece que el metodo mas efectivo es por distancia
                    //return prisioner ref
                }
                else
                {
                    //todo encontrar la referencia a la base enemiga
                    //coger la referencia de la lista de las entidades
                    //return base ref
                }     
            }
            
            //solo para que no haya nulls
            return playerControlledEntites[0];

        }
    }
}