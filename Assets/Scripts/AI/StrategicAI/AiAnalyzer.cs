using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AI.StrategicAI
{
    [System.Serializable]
    public class AiAnalyzer
    {
        //tareas asignadas en funcion de la personalidad a la IA.
        [SerializeField] private StrategicObjectives StrategicObjectives;      
        [SerializeField] private HighLevelAI _highLevelAI;
        [SerializeField] private List<AiTask> _selectableTasks;

        [Inject]
        public AiAnalyzer(HighLevelAI highLevelAi, StrategicObjectives objectives)
        {
            _highLevelAI = highLevelAi;
            _selectableTasks = new List<AiTask>();
            StrategicObjectives = objectives;
            OnPersonalityChanged();
        }

        //Esta clase se encarga de generar la lista de tareas para el resource allocator teniendo en cuenta el mapa de influencias
        public void GenerateTasks()
        {
            Entity[] controlledEntities = _highLevelAI.ControlledEntites.ToArray();
            
            //recorrerlas y hacer comprobaciones de cada unidad para saber que tarea tiene que componer, con que objetivo.
        }

        public void OnPersonalityChanged()
        {
            _selectableTasks.Clear();
            
            foreach (var task in StrategicObjectives.TasksDictionary.Values)
            {
                if(task.IaPersonality == _highLevelAI.CurrentIaPersonality)
                    _selectableTasks.Add(task);
            } 
        }
    }
}