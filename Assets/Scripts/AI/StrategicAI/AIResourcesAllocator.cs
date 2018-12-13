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
        public class PossibleTaskAssignment : IComparable<PossibleTaskAssignment>
        {
            private TacticalObjective _objective;
            public float score;
            public Entity PossibleTaskDoer { get; set; }

            public bool IsAssigned
            {
                get { return PossibleTaskDoer != null; }
            }

            public PossibleTaskAssignment(TacticalObjective objective)
            {
                _objective = objective;
                score = objective.Distance;
            }

            public int CompareTo(PossibleTaskAssignment other)
            {
                return score.CompareTo(other.score);
            }
        }

        public void OnTasksGenerated(List<TacticalObjective> tacticalObjectives, Entity[] controlledEntities)
        {
            //rehacer esto
            var possibleTaskAssignments = GenerateAllPossibleTasksAssignments(tacticalObjectives, controlledEntities);
            AssignPossibleAssignments(possibleTaskAssignments);
        }

        private void AssignPossibleAssignments(List<PossibleTaskAssignment> possibleTaskAssignments)
        {
            foreach (var possibleTaskAssignment in possibleTaskAssignments)
            {
                // al estar ordenadas las primeras tareas se asignan primero, si algun doer esta ocupado se le salta;
                
            }
            
            OnResourcesAllocated(possibleTaskAssignments);
        }

        private List<PossibleTaskAssignment> GenerateAllPossibleTasksAssignments(List<TacticalObjective> objectives, Entity[] controlledEntities)
        {
            List<PossibleTaskAssignment> possibleTaskAssignments = new List<PossibleTaskAssignment>();
            
            for (int i = 0; i < objectives.Count; i++)
            {
                for (int j = 0; j < controlledEntities.Length; j++)
                {
                    possibleTaskAssignments.Add(new PossibleTaskAssignment(objectives[i]));
                }
            }
            
            possibleTaskAssignments.Sort();

            return possibleTaskAssignments;
        }
        
        public void OnResourcesAllocated(List<PossibleTaskAssignment> possibleTaskAssignments)
        {
            //the assignments are done and now we have to iterate through each unity and depending on its task use a command or other
            for (int i = 0; i < possibleTaskAssignments.Count; i++)
            {
                if (possibleTaskAssignments[i].IsAssigned)
                {
                    //si el objetivo es de la IA sera mejora
                    
                    //si es del enemigo tienes que atacar
                }
            }
            
        }

        public void OnTaskCommandsReceived(List<AITaskCommand> aiTaskCommands)
        {
            throw new NotImplementedException();
        }
    }
}