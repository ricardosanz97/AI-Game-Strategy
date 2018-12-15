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
            if (analyzedNPC.entityType == ENTITY.Launcher || analyzedNPC.entityType == ENTITY.Prisioner)
            {
                List<float> dataSum = new List<float>();

                float minDist = Mathf.Infinity;
                Entity minDistEntity = playerControlledEntites[0];

                float coreSum = 0f;
                float prisionerSum = 0f;
                float launcherSum = 0f;
                float tankSum = 0f;
            
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
                }

                if (coreSum == Mathf.Max(dataSum.ToArray())) // entonces vamos a por el prisioner
                {
                    foreach(Entity e in playerControlledEntites)
                    {
                        if(e.entityType == ENTITY.Core){
                            float dist = Vector3.Distance(e.transform.localPosition, analyzedNPC.transform.localPosition);

                            if (dist < minDist)
                            {
                                minDist = dist;
                                minDistEntity = e;
                            }
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
                return minDistEntity;
            }

            //solo para que no haya nulls
            return playerControlledEntites[0];
        }
    }
}