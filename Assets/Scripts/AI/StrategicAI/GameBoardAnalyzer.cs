using System;
using System.Collections.Generic;
using System.Net;
using InfluenceMap;
using TMPro;
using UnityEngine;
using UnityEngine.VR;
using Zenject;

namespace StrategicAI
{   
    [System.Serializable]
    public class GameBoardAnalyzer
    {
        //tareas asignadas en funcion de la personalidad a la IA.
        [SerializeField] private HighLevelAI _highLevelAi;
        [SerializeField] private TasksAllocator _tasksAllocator;
        [SerializeField] private InfluenceMapComponent _influenceMapComponent;

        [Inject]
        public GameBoardAnalyzer(HighLevelAI highLevelAi, TasksAllocator tasksAllocator, InfluenceMapComponent influenceMapComponent)
        {
            _highLevelAi = highLevelAi;
            _tasksAllocator = tasksAllocator;
            _influenceMapComponent = influenceMapComponent;
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

            _tasksAllocator.OnTaskCommandsReceived(aiTaskCommands, controlledEntities);
        }

        private void AnalyzeSurroundingInfluences(List<AITaskCommand> aiTaskCommands,
            StrategicObjective chosenStrategicObjective, Entity analyzedEntity,
            Entity[] playerControlledEntites)
        {
            AbstractNPCBrain brain = analyzedEntity.GetComponent<AbstractNPCBrain>();

            if (brain != null) // es una entidad con brain, es decir, no es un muro
            {
                //get node of the entity in the influence map
                // look in a ring
                InfluenceMap.Node node = _influenceMapComponent.GetNodeAtLocation(brain.transform.position);
                List<Node> influenceData = _influenceMapComponent.GetKRingsOfNodes(node, chosenStrategicObjective.SampleRadius);
                
                Debug.Log(influenceData.Count);

                Entity chosenTarget = chosenStrategicObjective.DecideBasedOnInfluenceData(brain,influenceData);


                if(chosenTarget.owner == Entity.Owner.AI) //mejorar
                    aiTaskCommands.Add(new UpgradeAITaskCommand(chosenTarget));
                else if(chosenTarget.owner == Entity.Owner.Player)
                    aiTaskCommands.Add(new AttackAITaskCommand(brain,chosenTarget));
                else if (chosenTarget == null)
                    return;
                
            }
            else // es un muro
            {
                return;
            }
                
        }


    }
}