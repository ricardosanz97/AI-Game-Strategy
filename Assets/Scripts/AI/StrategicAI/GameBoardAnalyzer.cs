using System;
using System.Collections.Generic;
using System.Net;
using InfluenceMap;
using TMPro;
using UnityEngine;
using UnityEngine.VR;
using Zenject;
using Object = UnityEngine.Object;

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
            //declaramos la lisa de pequeños comandos que realizara la ia
            List<AITaskCommand> aiTaskCommands = new List<AITaskCommand>();

            LevelController levelController = Object.FindObjectOfType<LevelController>();
            Entity[] IAcontrolledEntities = levelController.AIEntities.ToArray();
            Entity[] playerControlledEntites = levelController.PlayerEntities.ToArray();

            //check your controlled entities and see the influences they have in their surroundings
            //en funcion del objetivo estrategico se fijaran las tareas que podran ser de un tipo o de otro.
            for (int i = 0; i < IAcontrolledEntities.Length; i++)
            {
                AnalyzeSurroundingInfluences(aiTaskCommands,chosenStrategicObjective, IAcontrolledEntities[i], playerControlledEntites, levelController);
            }

            Debug.Log(aiTaskCommands.Count);
            _tasksAllocator.OnTaskCommandsReceived(aiTaskCommands, IAcontrolledEntities);
        }

        private void AnalyzeSurroundingInfluences(List<AITaskCommand> aiTaskCommands,
            StrategicObjective chosenStrategicObjective, Entity analyzedEntity,
            Entity[] playerControlledEntites, LevelController levelController)
        {
            AbstractNPCBrain brain = analyzedEntity.GetComponent<AbstractNPCBrain>();

            if (brain != null) // es una entidad con brain, es decir, no es un muro
            {
                //get node of the entity in the influence map
                // look in a ring
                InfluenceMap.Node node = _influenceMapComponent.GetNodeAtLocation(brain.transform.position);
                List<Node> influenceData = _influenceMapComponent.GetKRingsOfNodes(node, chosenStrategicObjective.SampleRadius);

                Entity chosenTarget = chosenStrategicObjective.DecideBasedOnInfluenceData(brain,influenceData,playerControlledEntites,levelController);

                if (chosenTarget == null) //no hay nadie a quien atacar o mejorar, pues movemos.
                {
                    Debug.Log("trying to move");
                    aiTaskCommands.Add(new MoveAITaskCommand(brain));
                }
                else if (chosenTarget.owner == Entity.Owner.AI)
                {
                    //mejorar
                    Debug.Log("trying to upgrade");
                    aiTaskCommands.Add(new UpgradeAITaskCommand(chosenTarget));
                }
                else if (chosenTarget.owner == Entity.Owner.Player)
                {
                    Debug.Log("trying to attack");
                    aiTaskCommands.Add(new AttackAITaskCommand(brain, chosenTarget));
                }
            }
            else // es un muro
            {
                return;
            }
                
        }


    }
}