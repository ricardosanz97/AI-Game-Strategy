using System.Collections.Generic;
using InfluenceMap;
using StrategicAI;
using UnityEngine;

namespace StrategicAI
{
    public class AttackTroopsObjective : StrategicObjective
    {
        public override Entity DecideBasedOnInfluenceData(AbstractNPCBrain analyzedNPC, List<Node> influenceData,
            Entity[] playerControlledEntites)
        {
            List<float> dataSum = new List<float>();

            float minDist = Mathf.Infinity;
            Entity minDistEntity = playerControlledEntites[0];

            float coreSum = 0f;
            float prisionerSum = 0f;
            float launcherSum = 0f;
            float tankSum = 0f;
            float turretSum = 0f;

            //calculo todas las influencias cercanas del npc

            foreach (var node in influenceData)
            {
                if (node.HasInfluenceOfType(InfluenceType.Core))
                {
                    coreSum += node.GetInfluenceOfType(InfluenceType.Core).Value;
                    dataSum.Add(coreSum);
                }
                else if (node.HasInfluenceOfType(InfluenceType.Prisioner))
                {
                    prisionerSum += node.GetInfluenceOfType(InfluenceType.Prisioner).Value;
                    dataSum.Add(prisionerSum);
                }
                else if (node.HasInfluenceOfType(InfluenceType.Launcher))
                {
                    launcherSum += node.GetInfluenceOfType(InfluenceType.Launcher).Value;
                    dataSum.Add(launcherSum);
                }
                else if (node.HasInfluenceOfType(InfluenceType.Tank))
                {
                    tankSum += node.GetInfluenceOfType(InfluenceType.Tank).Value;
                    dataSum.Add(tankSum);
                }
                else if (node.HasInfluenceOfType(InfluenceType.Turret))
                {
                    turretSum += node.GetInfluenceOfType(InfluenceType.Turret).Value;
                    dataSum.Add(turretSum);
                }
            }

            //si el npc es launcher o prisioner podran atacar a todo tipo de entidades, ya sean tropas, torretas o el core

            if (analyzedNPC.entityType == ENTITY.Launcher || analyzedNPC.entityType == ENTITY.Prisioner)
            {                            
                if (coreSum == Mathf.Max(dataSum.ToArray())) 
                {
                    foreach(Entity e in playerControlledEntites)
                    {
                        if(e.entityType == ENTITY.Core){
                            minDistEntity = e;
                        }
                    }
                }

                else if(launcherSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Launcher)
                        {
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
                        }

                    }
                }

                else if (prisionerSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Prisioner)
                        {
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
                        }

                    }
                }

                else if (tankSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Tank)
                        {
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
                        }
                    }
                }

                else if (turretSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Turret)
                        {
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
                        }
                    }
                }
                return minDistEntity;
            }

            //si es tanque, solo podra atacar al core y a torretas
            
            else if(analyzedNPC.entityType == ENTITY.Tank)
            {
                dataSum.Remove(launcherSum);
                dataSum.Remove(prisionerSum);
                dataSum.Remove(tankSum);

                if (coreSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Core)
                        {
                            minDistEntity = e;
                        }
                    }
                }

                else if (turretSum == Mathf.Max(dataSum.ToArray()))
                {
                    foreach (Entity e in playerControlledEntites)
                    {
                        if (e.entityType == ENTITY.Turret)
                        {
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
                        }
                    }
                }
            }
            //solo para que no haya nulls
            return playerControlledEntites[0];
        }
    }
}