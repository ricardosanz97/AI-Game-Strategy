using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Zenject;

namespace StrategicAI
{
    public class HighLevelAI : MonoBehaviour
    {
        [SerializeField] private StrategicObjective _strategicObjective;      
        [Inject] private GameBoardAnalyzer _analyzer;
        [Inject] private TurnHandler _turnHandler;
        public List<Entity> AIControlledEntites;
        public List<Entity> PlayerControlledEntities;
        [SerializeField] private CellBehaviour[] _spawnableCells;

        public CellBehaviour[] SpawnableCells => _spawnableCells;

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
            AIControlledEntites = new List<Entity>();
            PlayerControlledEntities = new List<Entity>();
            Assert.IsNotNull(_spawnableCells);
        }

        [ContextMenu("Evaluate Game State")]
        public void EvaluateGameState()
        {
            _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
            
            //todo establecer las diferentes reglas para cambiar entre objetivos estrategicos
            if (CalculateSetDamage(AIControlledEntites) >= CalculateSetDamage(PlayerControlledEntities))
                _strategicObjective = GetOrAddComponent<AttackBaseObjective>();
            else
                _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();

            _analyzer.AnalyzeGameTerrain(_strategicObjective);
            Debug.Log("Chosen Strategic Objective: " + _strategicObjective);
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

        private T GetOrAddComponent<T>() where T : MonoBehaviour
        {
            T component = GetComponent<T>();

            return component ? component : gameObject.AddComponent<T>();
        }
    }
}