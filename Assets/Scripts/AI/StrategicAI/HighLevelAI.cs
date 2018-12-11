using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Zenject;

namespace AI.StrategicAI
{
    public class HighLevelAI : MonoBehaviour
    {
        public enum IAPersonality
        {
            Offensive,
            Defensive
        }

        [SerializeField] private IAPersonality currentPersonality;
        [SerializeField] private StrategicObjectives StrategicObjectives;      
        [Inject] private AiAnalyzer _analyzer;
        [Inject] private TurnHandler _turnHandler;
        public List<Entity> AIControlledEntites;
        public List<Entity> PlayerControlledEntities;
        
        public IAPersonality CurrentIaPersonality { get; private set; }

        private void OnEnable()
        {
            SpawnablesManager.OnSpawnedTroop += RegisterSpawnedEntity;
            Entity.OnTroopDeleted += UnregisterDeletedEntity;
        }

        private void OnDisable()
        {
            SpawnablesManager.OnSpawnedTroop -= RegisterSpawnedEntity;
            Entity.OnTroopDeleted += UnregisterDeletedEntity;
        }

        private void Start()
        {
            CurrentIaPersonality = IAPersonality.Offensive;
            AIControlledEntites = new List<Entity>();
            PlayerControlledEntities = new List<Entity>();
        }

        public void EvaluateGameState()
        {
            if (CalculateSetDamage(AIControlledEntites) >= CalculateSetDamage(PlayerControlledEntities))
                currentPersonality = IAPersonality.Offensive;
            else
                currentPersonality = IAPersonality.Defensive;
            
            //SendObjectivesToAnalyzer(StrategicObjectives.TasksDictionary[currentPersonality].objectives);

        }

        private void SendObjectivesToAnalyzer(Objective[] objectives)
        {
            _analyzer.GenerateTasks(objectives);
        }

        public void PlayTurn()
        {
            Debug.Log("Ia Player turn");
            EvaluateGameState();
            _turnHandler.AIDone = true;
        }


        private void RegisterSpawnedEntity(Entity e)
        {
            if(e.owner == Entity.Owner.AI && !AIControlledEntites.Contains(e))
                AIControlledEntites.Add(e);
            else if(e.owner == Entity.Owner.Player && !PlayerControlledEntities.Contains(e))
                PlayerControlledEntities.Add(e);    
        }
        
        private void UnregisterDeletedEntity(Entity e)
        {
            if (e.owner == Entity.Owner.AI && AIControlledEntites.Contains(e))
                AIControlledEntites.Remove(e);
            else if (e.owner == Entity.Owner.Player && PlayerControlledEntities.Contains(e))
                PlayerControlledEntities.Remove(e);
                
        }

        private int CalculateSetDamage(List<Entity> entities)
        {
            int totalSum = 0;
            
            foreach (var entity in entities)
            {
                totalSum += (int)entity.value;
            }

            return totalSum;
        }

    }
}