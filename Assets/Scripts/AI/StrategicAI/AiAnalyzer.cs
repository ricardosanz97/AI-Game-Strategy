using System.Collections.Generic;
using InfluenceMap;
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
        [SerializeField] private AIResourcesAllocator _aiResourcesAllocator;
        [SerializeField] private List<AiTask> _selectableTasks;
        [SerializeField] private InfluenceMapComponent _influenceMapComponent;

        [Inject]
        public AiAnalyzer(HighLevelAI highLevelAi, AIResourcesAllocator aiResourcesAllocator, StrategicObjectives objectives, InfluenceMapComponent influenceMapComponent)
        {
            _highLevelAI = highLevelAi;
            _aiResourcesAllocator = aiResourcesAllocator;
            _influenceMapComponent = influenceMapComponent;
            _selectableTasks = new List<AiTask>();
            StrategicObjectives = objectives;
        }

        //Esta clase se encarga de generar la lista de tareas para el resource allocator teniendo en cuenta el mapa de influencias
        public void GenerateTasks()
        {
            Entity[] controlledEntities = _highLevelAI.AIControlledEntites.ToArray();
            Entity[] playerControlledEntites = _highLevelAI.PlayerControlledEntities.ToArray();
            
            List<AiTask> tasks = new List<AiTask>();
            
            //check your controlled entities and see the influences they have in their surroundings
            //dos opciones, o bien hacer un arbol de decision para añadir tareas o bien usar reglas
            
            
            _aiResourcesAllocator.OnTasksGenerated(tasks.ToArray(), controlledEntities);
        }

        public void OnPersonalityChanged()
        {
            _selectableTasks.Clear();
            
            foreach (var task in StrategicObjectives.TasksDictionary.Values)
            {
                if(task.IaPersonality == _highLevelAI.CurrentIaPersonality)
                    _selectableTasks.Add(task);
            } 
            
            GenerateTasks();
        }
    }
}