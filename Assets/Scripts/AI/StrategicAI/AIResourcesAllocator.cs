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
        public void OnTaskCommandsReceived(List<AITaskCommand> aiTaskCommands)
        {
            for (int i = 0; i < aiTaskCommands.Count; i++)
            {
                aiTaskCommands[i].PerformCommand();
            }

        }
    }
}
