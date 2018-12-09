using System.Collections.Generic;
using UnityEngine;

namespace AI.StrategicAI
{
    [System.Serializable]
    public class AIResourcesAllocator
    {
        private class PossibleTaskAssignment
        {
            private AiTask _task;

            public PossibleTaskAssignment(AiTask task)
            {
                _task = task;
            }
        }

        private List<PossibleTaskAssignment> _possibleTaskAssignments;
        
        public void OnTasksGenerated(AiTask[] tasks, Entity[] controlledEntities)
        {
            _possibleTaskAssignments = new List<PossibleTaskAssignment>();
            
            for (int i = 0; i < tasks.Length; i++)
            {
                for (int j = 0; j < controlledEntities.Length; j++)
                {
                    if (controlledEntities[j].isTaskSuitable(tasks[i]))
                    {
                        _possibleTaskAssignments.Add(new PossibleTaskAssignment(tasks[i]));
                    }
                }
            }
        }
    }
}