using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace AI.StrategicAI
{
    [System.Serializable]
    public class AIResourcesAllocator
    {
        public class PossibleTaskAssignment : IComparable<PossibleTaskAssignment>
        {
            public AiTask Task { get;}
            public float score;
            public Entity PossibleTaskDoer { get; private set; }

            public PossibleTaskAssignment(AiTask task)
            {
                Task = task;
                score = 0;
            }

            public void Assign()
            {
                if (Task.IsAssigned()) return;
                PossibleTaskDoer.Assign(this);
            }

            public int CompareTo(PossibleTaskAssignment other)
            {
                //testear si no es capaz de ordenar las tareas
                return score.CompareTo(other.score);
            }
        }

        private List<PossibleTaskAssignment> _possibleTaskAssignments;
        private HighLevelAI _highLevelAi;

        public AIResourcesAllocator(List<PossibleTaskAssignment> possibleTaskAssignments, HighLevelAI highLevelAi)
        {
            _possibleTaskAssignments = possibleTaskAssignments;
            _highLevelAi = highLevelAi;
        }

        public void OnTasksGenerated(AiTask[] tasks, Entity[] controlledEntities)
        {
            GenerateAllPossibleTasksAssignments(tasks, controlledEntities);
            SortPossibleAssignments(_possibleTaskAssignments);
            AssignPossibleAssignments(_possibleTaskAssignments);

            //send the commands
            OnResourcesAllocated();

        }

        private void AssignPossibleAssignments(List<PossibleTaskAssignment> possibleTaskAssignments)
        {
            foreach (var possibleTaskAssignment in possibleTaskAssignments)
            {
                // al estar ordenadas las primeras tareas se asignan primero, si algun doer esta ocupado se le salta;
                possibleTaskAssignment.Assign();
            }
        }

        private void SortPossibleAssignments(List<PossibleTaskAssignment> possibleTaskAssignments)
        {
            for (int i = 0; i < possibleTaskAssignments.Count; i++)
            {
                //calculate the score of each assignment
            }
            
            possibleTaskAssignments.Sort();
        }

        private void GenerateAllPossibleTasksAssignments(AiTask[] tasks, Entity[] controlledEntities)
        {
            _possibleTaskAssignments.Clear();

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
        
        public void OnResourcesAllocated()
        {
            //the assignments are done and now we have to iterate through each unity and depending on its task use a command or other
            
        }
    }
}