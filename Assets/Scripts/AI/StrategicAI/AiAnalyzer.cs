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
        public float Modifier { get; }

        public TacticalObjective(Entity objective, float modifier)
        {
            this.Objective = objective;
            this.Modifier = modifier;
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
            //input las tareas para esta personalidad con prioridades asociadas, las ordenamos para que genere tareas primero para las prioridades altas
            Array.Sort(aiObjectives);
            List<TacticalObjective> specificObjectives = new List<TacticalObjective>();
            
            Entity[] controlledEntities = _highLevelAI.AIControlledEntites.ToArray();
            Entity[] playerControlledEntites = _highLevelAI.PlayerControlledEntities.ToArray();

            for (int i = 0; i < aiObjectives.Length; i++)
            {
                for (int j = 0; j < controlledEntities.Length; j++)
                {
                    AnalyzeSurroundingInfluences(controlledEntities[i], aiObjectives[i]);
                }
            }

           
            
            //check your controlled entities and see the influences they have in their surroundings
            //dos opciones, o bien hacer un arbol de decision para añadir tareas o bien usar reglas
            
            
            _aiResourcesAllocator.OnTasksGenerated(specificObjectives, controlledEntities);
        }

        private void AnalyzeSurroundingInfluences(Entity e, AIObjective objective)
        {
            AbstractNPCBrain brain = e.GetComponent<AbstractNPCBrain>();
            
            switch (objective.objectiveType)
            {
                //genera tareas que pone entidades o celdas como objetivo el peso que tengan dependeran de si le hacemos counter o si estan debilitadas 
                case AIObjective.ObjectiveType.AttackBase:
                        //genera tareas a las unidades de avanzar y de atacar a la ba
                    break;
                    
                case AIObjective.ObjectiveType.AttackDefenses:
                        //genera tareas de atacar si estan cerca de muros y de moverse si estan lejos
                    break;
                    
                case AIObjective.ObjectiveType.AttackTroops:
                        //genera tareas de atacar a cualquier tropa que este cerca y si no se acerca a ellas
                    break;
                    
                case AIObjective.ObjectiveType.DefendBase:
                        //genera tareas de mejora de estructuras y de ataque de enemigos, sin moverse
                    break;
                    
                case AIObjective.ObjectiveType.ProtectTroops:
                        //genera tareas para moverse hacia atras
                    break;
                    
                case AIObjective.ObjectiveType.UpgradeDefenses:
                        //genera tareas de mejora de estructuras
                    break;
            }
        }

        public void OnPersonalityChanged()
        {

        }
        
    }
}