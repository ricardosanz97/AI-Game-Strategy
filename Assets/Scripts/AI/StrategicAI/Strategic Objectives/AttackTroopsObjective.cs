using System.Collections.Generic;
using System.Linq;
using InfluenceMap;
using StrategicAI;
using UnityEngine;

namespace StrategicAI
{
    public class AttackTroopsObjective : StrategicObjective
    {
        public override Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNPC, List<Node> influenceData, Entity[] playerControlledEntites, LevelController levelController)
        {
            List<float> dataSum = new List<float>();
            Entity minDistEntity = null;

            float coreSum = 0f;
            float prisionerSum = 0f;
            float launcherSum = 0f;
            float tankSum = 0f;
            float turretSum = 0f;

            //calculo todas las influencias cercanas del npc
            foreach (var node in influenceData)
            {
                if (node.HasInfluenceOfType(InfluenceType.Core))
                    coreSum += node.GetInfluenceOfType(InfluenceType.Core).Value;
                else if (node.HasInfluenceOfType(InfluenceType.Prisioner))
                    prisionerSum += node.GetInfluenceOfType(InfluenceType.Prisioner).Value;
                else if (node.HasInfluenceOfType(InfluenceType.Launcher))
                    launcherSum += node.GetInfluenceOfType(InfluenceType.Launcher).Value;
                else if (node.HasInfluenceOfType(InfluenceType.Tank))
                    tankSum += node.GetInfluenceOfType(InfluenceType.Tank).Value;
                else if (node.HasInfluenceOfType(InfluenceType.Turret))
                    turretSum += node.GetInfluenceOfType(InfluenceType.Turret).Value;
                
                //todo influencia del core
            }
            
            dataSum.Add(coreSum);
            dataSum.Add(prisionerSum);
            dataSum.Add(launcherSum);
            dataSum.Add(tankSum);
            dataSum.Add(turretSum);
            
            //si el npc es launcher o prisioner podran atacar a todo tipo de entidades, ya sean tropas, torretas o el core
            if (analyzedNPC.entityType == ENTITY.Launcher || analyzedNPC.entityType == ENTITY.Prisioner)
            {                            
                if (coreSum == Mathf.Max(dataSum.ToArray())) 
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, levelController.playerCoreEntities.ToArray() , ENTITY.Core);
                else if(launcherSum == Mathf.Max(dataSum.ToArray()))
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, playerControlledEntites, ENTITY.Launcher);
                else if (prisionerSum == Mathf.Max(dataSum.ToArray()))
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, playerControlledEntites, ENTITY.Prisioner);
                else if (tankSum == Mathf.Max(dataSum.ToArray()))
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, playerControlledEntites, ENTITY.Tank);
                else if (turretSum == Mathf.Max(dataSum.ToArray()))
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, playerControlledEntites, ENTITY.Turret);
                
                if(minDistEntity != null)
                {
                    analyzedNPC.GetComponent<Troop>().GetCellsWithEnemiesInRange();
            
                    if(analyzedNPC.GetComponent<Troop>().possibleAttacks.Contains(minDistEntity.cell.PNode))
                        return minDistEntity;
                }

            }
            else if(analyzedNPC.entityType == ENTITY.Tank) //si es tanque, solo podra atacar al core y a torretas
            {
                dataSum.Remove(launcherSum);
                dataSum.Remove(prisionerSum);
                dataSum.Remove(tankSum);

                if (coreSum == Mathf.Max(dataSum.ToArray()))
                {
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, levelController.playerCoreEntities.ToArray());
                }

                else if (turretSum == Mathf.Max(dataSum.ToArray()))
                {
                    minDistEntity = GetClosestEntityInCollection(analyzedNPC, playerControlledEntites, ENTITY.Turret);
                }
                
                if(minDistEntity != null)
                {
                    analyzedNPC.GetComponent<Troop>().GetCellsWithEnemiesInRange();
            
                    if(analyzedNPC.GetComponent<Troop>().possibleAttacks.Contains(minDistEntity.cell.PNode))
                        return minDistEntity;
                }

            }
            
            return null;
        }
    }
}