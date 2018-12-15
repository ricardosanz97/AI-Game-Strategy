using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace StrategicAI
{
    [System.Serializable]
    public class TasksAllocator
    {
        [Inject]private TurnHandler _turnHandler;
        [Inject] private HighLevelAI _highLevelAi;
        
        public void OnTaskCommandsReceived(List<AITaskCommand> aiTaskCommands, Entity[] controlledEntities)
        {
            //analiar los recursos de sangre que tenemos
            //en funcion de esos recursos y las tareas que tenemos decidir
            
            //antes de llamar a todos lo comandos hay que ver si hay una desproporcion entre tareas
            //y tropas controladas por nosotros
            //todo cambiar el magic number por un parametro desde el high level
            if (IsSpawnNeeded(aiTaskCommands, controlledEntities, 2))
                DecideWhatToSpawn(aiTaskCommands);
            
            for (int i = 0; i < aiTaskCommands.Count; i++)
            {
                //el perform command se encarga de o bien comunicarle a la fsm
                //la señal necesaria como ataque o defensa
                aiTaskCommands[i].PerformCommand();
                Debug.Log("Performing Command");
            }
            
            //añadirlo a una cola en algun monobehaviour para que una corutina lo vaya sacando poco a poco.

            Debug.Log("AI Done");
            _turnHandler.AIDone = true;

        }

        private bool IsSpawnNeeded(List<AITaskCommand> aiTaskCommands, Entity[] controlledEntities, int threshhold)
        {
            return Mathf.Abs(controlledEntities.Length - aiTaskCommands.Count) > threshhold || controlledEntities.Length == 0 || aiTaskCommands.Count == 0;
        }

        private void DecideWhatToSpawn(List<AITaskCommand> aiTaskCommands)
        {
            Debug.Log("spawning");
            if (HasResourcesToSpawn())
            {
                //decide what to spawn and add it to the aitaskcommand
                Assert.IsNotNull(_highLevelAi.SpawnableCells);
                SpawnAITaskCommand spawnCommand = new SpawnAITaskCommand(ENTITY.Launcher, _highLevelAi.SpawnableCells);
                aiTaskCommands.Insert(0,spawnCommand);  
            }
        }

        private bool HasResourcesToSpawn()
        {
            //todo meter el contador de la sangre en el high level
            return true;
        }
    }
}
