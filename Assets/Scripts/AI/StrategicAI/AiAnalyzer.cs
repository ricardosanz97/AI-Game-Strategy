using System;
using System.Collections.Generic;
using InfluenceMap;
using UnityEngine;
using Zenject;

namespace StrategicAI
{
    public class TacticalObjective
    {
        public Entity Objective { get; }
        public float Distance { get; set; }

        public TacticalObjective(Entity objective, float distance)
        {
            this.Objective = objective;
            this.Distance = distance;
        }
    }
    
    [System.Serializable]
    public class AiAnalyzer
    {
        //tareas asignadas en funcion de la personalidad a la IA.
        [SerializeField] private HighLevelAI _highLevelAI;
        [SerializeField] private AIResourcesAllocator _aiResourcesAllocator;
        [SerializeField] private InfluenceMapComponent _influenceMapComponent;

        [Inject]
        public AiAnalyzer(HighLevelAI highLevelAi, AIResourcesAllocator aiResourcesAllocator, InfluenceMapComponent influenceMapComponent)
        {
            _highLevelAI = highLevelAi;
            _aiResourcesAllocator = aiResourcesAllocator;
            _influenceMapComponent = influenceMapComponent;
        }

        //Esta clase se encarga de generar la lista de tareas para el resource allocator teniendo en cuenta el mapa de influencias
        public void GenerateTasks(AIObjective[] aiObjectives)
        {
            
        }

        private void AnalyzeSurroundingInfluences(Entity e,Entity[] playerControlledEntites)
        {
            AbstractNPCBrain brain = e.GetComponent<AbstractNPCBrain>();
        }

        private static void GetClosestTacticalObjective(Entity e, Entity[] playerControlledEntites, List<TacticalObjective> specificObjectives)
        {
            float currentDistance = 0;
            float minDistance = int.MaxValue;
            Entity closestEntity = null;

            for (int i = 0; i < playerControlledEntites.Length; i++)
            {
                currentDistance = Vector3.Distance(e.transform.position, playerControlledEntites[i].transform.position);
                if (currentDistance < minDistance)
                {
                    closestEntity = playerControlledEntites[i];
                    minDistance = currentDistance;
                }
            }

            specificObjectives.Add(new TacticalObjective(closestEntity, currentDistance));
        }

        public void AnalyzeGameTerrain(StrategicObjective chosenStrategicObjective)
        {   
            List<AITaskCommand> aiTaskCommands = new List<AITaskCommand>();
            
            Entity[] controlledEntities = _highLevelAI.AIControlledEntites.ToArray();
            Entity[] playerControlledEntites = _highLevelAI.PlayerControlledEntities.ToArray();

            for (int i = 0; i < controlledEntities.Length; i++)
            {
                AnalyzeSurroundingInfluences(controlledEntities[i], playerControlledEntites);
            }


            //check your controlled entities and see the influences they have in their surroundings
            //dos opciones, o bien hacer un arbol de decision para añadir tareas o bien usar reglas


            _aiResourcesAllocator.OnTaskCommandsReceived(aiTaskCommands);
        }
    }
}