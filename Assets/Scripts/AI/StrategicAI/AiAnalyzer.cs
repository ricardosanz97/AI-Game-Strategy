using System;
using System.Collections.Generic;
using System.Net;
using InfluenceMap;
using TMPro;
using UnityEngine;
using Zenject;

namespace StrategicAI
{   
    [System.Serializable]
    public class AiAnalyzer
    {
        //tareas asignadas en funcion de la personalidad a la IA.
        [SerializeField] private HighLevelAI _highLevelAi;
        [SerializeField] private AIResourcesAllocator _aiResourcesAllocator;
        [SerializeField] private InfluenceMapComponent _influenceMapComponent;

        [Inject]
        public AiAnalyzer(HighLevelAI highLevelAi, AIResourcesAllocator aiResourcesAllocator, InfluenceMapComponent influenceMapComponent)
        {
            _highLevelAi = highLevelAi;
            _aiResourcesAllocator = aiResourcesAllocator;
            _influenceMapComponent = influenceMapComponent;
        }

        private void AnalyzeSurroundingInfluences(List<AITaskCommand> aiTaskCommands,
            StrategicObjective chosenStrategicObjective, Entity e,
            Entity[] playerControlledEntites)
        {
            AbstractNPCBrain brain = e.GetComponent<AbstractNPCBrain>();

            if (brain != null) // es una entidad con brain, es decir, no es un muro
            {
                //get node of the entity in the influence map
                // look in a ring
                InfluenceMap.Node node = _influenceMapComponent.GetNodeAtLocation(brain.transform.position);
                List<Node> influenceData = _influenceMapComponent.GetKRingsOfNodes(node, chosenStrategicObjective.SampleRadius);

                Entity[] chosenTargets = chosenStrategicObjective.DecideBasedOnInfluenceData(influenceData);


                for (int i = 0; i < chosenTargets.Length; i++)
                {
                    if(chosenTargets[i].owner == Entity.Owner.AI) //mejorar
                        aiTaskCommands.Add(new UpgradeAITaskCommand());
                    else
                        aiTaskCommands.Add(new AttackAITaskCommand());
                }
            }
            else // es un muro
            {
                return;
            }
                
        }

        private static void GetClosestTacticalObjective(Entity e, Entity[] playerControlledEntites)
        {
            float currentDistance = 0;
            float minDistance = int.MaxValue;
            Entity closestEntity = null;

            for (int i = 0; i < playerControlledEntites.Length; i++)
            {
                currentDistance = Vector3.Distance(e.transform.position, playerControlledEntites[i].transform.position);
                if (currentDistance < minDistance)
                {
                    closestEntity = playerControlledEntites[i];
                    minDistance = currentDistance;
                }
            }
        }

        public void AnalyzeGameTerrain(StrategicObjective chosenStrategicObjective)
        {   
            List<AITaskCommand> aiTaskCommands = new List<AITaskCommand>();
            
            Entity[] controlledEntities = _highLevelAi.AIControlledEntites.ToArray();
            Entity[] playerControlledEntites = _highLevelAi.PlayerControlledEntities.ToArray();

            //check your controlled entities and see the influences they have in their surroundings
            //en funcion del objetivo estrategico se fijaran las tareas que podran ser de un tipo o de otro.
            for (int i = 0; i < controlledEntities.Length; i++)
            {
                AnalyzeSurroundingInfluences(aiTaskCommands,chosenStrategicObjective, controlledEntities[i], playerControlledEntites);
            }

            _aiResourcesAllocator.OnTaskCommandsReceived(aiTaskCommands, controlledEntities);
        }
    }
}