using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;
using Random = UnityEngine.Random;
using StrategicAI;
using Object = UnityEngine.Object;

namespace StrategicAI
{
    [System.Serializable]
    public class TasksAllocator
    {
        [Inject]private TurnHandler _turnHandler;
        [Inject] private HighLevelAI _highLevelAi;
        [Inject] private BloodController _bloodController;
        
        public void OnTaskCommandsReceived(List<AITaskCommand> aiTaskCommands, Entity[] controlledEntities, AISpawnStrategy aISpawnStrategy)
        {
            LevelController _levelController =  Object.FindObjectOfType<LevelController>();
            
            //analiar los recursos de sangre que tenemos
            //en funcion de esos recursos y las tareas que tenemos decidir
            
            //antes de llamar a todos lo comandos hay que ver si hay una desproporcion entre tareas
            //y tropas controladas por nosotros
            //todo cambiar el magic number por un parametro desde el high level
            if (IsSpawnNeeded(aiTaskCommands, _levelController.AIEntities.ToArray(), _levelController.PlayerEntities.ToArray() , 2))
                DecideWhatToSpawn(aiTaskCommands, aISpawnStrategy);
            
            _highLevelAi.StartCoroutine(PerformDifferentTasks(aiTaskCommands));
            
            //añadirlo a una cola en algun monobehaviour para que una corutina lo vaya sacando poco a poco.

            Debug.Log("AI Done");
            //_turnHandler.AIDone = true;

        }

        IEnumerator PerformDifferentTasks(List<AITaskCommand> aiTaskCommands)
        {
            for (int i = 0; i < aiTaskCommands.Count; i++)
            {
                //el perform command se encarga de o bien comunicarle a la fsm
                //la señal necesaria como ataque o defensa
                aiTaskCommands[i].PerformCommand();
                Debug.Log("Performing Command");
                yield return new WaitForSeconds(2.0f);
            }   
            
            //todo mostrar letrero
            _turnHandler.AIDone = true;
            Debug.Log("AI Done");
            yield return null;
        }
        

        private bool IsSpawnNeeded(List<AITaskCommand> aiTaskCommands, Entity[] AIEntities, Entity[] playerEntities,
            int threshhold)
        {
            return Mathf.Abs(playerEntities.Length - AIEntities.Length) >= threshhold || playerEntities.Length == 0 || aiTaskCommands.Count == 0 || AIEntities.Length == 0;
        }

        private void DecideWhatToSpawn(List<AITaskCommand> aiTaskCommands, AISpawnStrategy strategySpawn)
        {
            int currentBlood = _bloodController.GetCurrentAIBlood();
            List<EntityBloodCost> costs = strategySpawn.entitiesBloodCost;

            for (int i = 0; i < costs.Count; i++)
            {
                if (costs[i].bloodCost <= currentBlood)
                {
                    SpawnAITaskCommand spawnCommand = new SpawnAITaskCommand(costs[i].entity, _highLevelAi.SpawnableCells);
                    aiTaskCommands.Insert(0, spawnCommand);
                    currentBlood -= costs[i].bloodCost;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
