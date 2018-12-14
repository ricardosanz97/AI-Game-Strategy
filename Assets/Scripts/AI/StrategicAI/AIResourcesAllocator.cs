using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Zenject;

namespace StrategicAI
{
    [System.Serializable]
    public class AIResourcesAllocator
    {
        public void OnTaskCommandsReceived(List<AITaskCommand> aiTaskCommands, Entity[] controlledEntities)
        {
            //analiar los recursos de sangre que tenemos
            //en funcion de esos recursos y las tareas que tenemos decidir
            
            //antes de llamar a todos lo comandos hay que ver si hay una desproporcion entre tareas
            //y tropas controladas por nosotros
            //todo cambiar el magic number por un parametro
            if (Mathf.Abs(controlledEntities.Length - aiTaskCommands.Count) > 3)
                DecideWhatToSpawn(aiTaskCommands);
            
            
            for (int i = 0; i < aiTaskCommands.Count; i++)
            {
                //el perform command se encarga de o bien comunicarle a la fsm
                //la señal necesaria como ataque o defensa
                aiTaskCommands[i].PerformCommand();
            }

        }

        private void DecideWhatToSpawn(List<AITaskCommand> aiTaskCommands)
        {
            //decide what to spawn and add it to the aitaskcommand
        }
    }
}
