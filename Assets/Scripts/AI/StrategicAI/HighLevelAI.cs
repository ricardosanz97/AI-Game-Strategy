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
        [SerializeField] private List<CellBehaviour> _spawnableCells;
        [SerializeField] private LevelController _levelController;

        public int entitiesValueAI;
        public int entitiesValuePlayer;

        public int entitiesNumberAI;
        public int entitiesNumberPlayer;

        public int entitiesValueAIOwnedPlayer;
        public int entitiesValuePlayerOwnedPlayer;

        public int entitiesValueAIOwnedAI;
        public int entitiesValuePlayerOwnedAI;
         
        public List<CellBehaviour> SpawnableCells => _spawnableCells;

        private void Start()
        {               
            _levelController = FindObjectOfType<LevelController>();
            Assert.IsNotNull(_levelController);
            var spawners = FindObjectsOfType<CellBehaviour>();

            foreach (var spawnableCell in spawners)
            {
                if(spawnableCell.GetComponent<CellBehaviour>().GetComponent<SpawnableCell>() != null && spawnableCell.GetComponent<CellBehaviour>().owner == PlayerType.AI)
                    _spawnableCells.Add(spawnableCell.GetComponent<CellBehaviour>());
            }
            
            Assert.IsNotNull(_spawnableCells);
        }

        [ContextMenu("Evaluate Game State")]
        public void EvaluateGameState()
        {
            List<Entity> AIEntitiesOwnedPlayer = castEntitiesOwner(_levelController.AIEntities, Entity.Owner.Player);
            List<Entity> PlayerEntitiesOwnedPlayer = castEntitiesOwner(_levelController.PlayerEntities, Entity.Owner.Player);

            List<Entity> AIEntitiesOwnedAI = castEntitiesOwner(_levelController.AIEntities, Entity.Owner.AI);
            List<Entity> PlayerEntitiesOwnedAI = castEntitiesOwner(_levelController.PlayerEntities, Entity.Owner.AI);

            entitiesValueAI = CalculateValueEntities(_levelController.AIEntities);
            entitiesValuePlayer = CalculateValueEntities(_levelController.PlayerEntities);

            entitiesNumberAI = _levelController.AIEntities.Count;
            entitiesNumberPlayer = _levelController.PlayerEntities.Count;

            entitiesValueAIOwnedPlayer = CalculateValueEntities(AIEntitiesOwnedPlayer);
            entitiesValuePlayerOwnedPlayer = CalculateValueEntities(PlayerEntitiesOwnedPlayer);

            entitiesValueAIOwnedAI = CalculateValueEntities(AIEntitiesOwnedAI);
            entitiesValuePlayerOwnedAI = CalculateValueEntities(PlayerEntitiesOwnedAI);

            if (entitiesValueAI >= entitiesValuePlayer)
            {
                if(entitiesNumberAI >= entitiesNumberPlayer)
                {
                    if(entitiesValueAIOwnedPlayer >= entitiesValuePlayerOwnedPlayer)
                    {
                        _strategicObjective = GetOrAddComponent<AttackBaseObjective>();
                        //Spawnear tropas
                    }
                    else
                    {
                        _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
                        //Spawnear tropas
                    }
                }
                else
                {
                    _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
                    //Spawnear tropas
                }

            }
            else{
                if (entitiesNumberAI >= entitiesNumberPlayer)
                {
                    _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
                    //Spawn muros o torretas; mejorarlos
                }
                else
                {
                    if (entitiesValueAIOwnedAI >= entitiesValuePlayerOwnedAI)
                    {
                        _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
                        //Spawnear tropas
                    }
                    else
                    {
                        _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
                        //Spawn muros o torretas; mejorarlos
                    }
                }
            }

            _analyzer.AnalyzeGameTerrain(_strategicObjective);
            Debug.Log("Chosen Strategic Objective: " + _strategicObjective);  
        }

        public void PlayTurn()
        {
            Debug.Log("Ia Player turn");
            EvaluateGameState();
            _turnHandler.AIDone = true;
        }

        private List<Entity> castEntitiesOwner(List<Entity> entities, Entity.Owner own)
        {
            List<Entity> entitiesCasted = new List<Entity>();
            foreach (Entity e in entities)
            {
                if(e.owner == own)
                {
                    entitiesCasted.Add(e);
                }
            }
            return entitiesCasted;
        }

        private int CalculateValueEntities(List<Entity> entities)
        {
            int sumEntityValues = 0;

            foreach (Entity e in entities)
            {
                int entityValue = 0;

                if (e.entityType == ENTITY.Launcher || e.entityType == ENTITY.Prisioner || e.entityType == ENTITY.Tank)
                {
                    entityValue += e.GetComponent<Health>().health;
                    entityValue += e.GetComponent<Attack>().damage;

                    sumEntityValues += entityValue;
                }
                else if (e.entityType == ENTITY.Core)
                {
                    entityValue += e.GetComponent<Health>().health;

                    sumEntityValues += entityValue;
                }
                else if(e.entityType == ENTITY.Turret)
                {
                    entityValue += e.GetComponent<Health>().health;
                    entityValue += e.GetComponent<AreaAttack>().damage;

                    sumEntityValues += entityValue;
                }
            }

            return sumEntityValues;

        }


        private void RegisterSpawnedEntity(Entity e)
        {
            if(e.owner == Entity.Owner.AI && !_levelController.AIEntities.Contains(e))
                _levelController.AIEntities.Add(e);
            else if(e.owner == Entity.Owner.Player && !_levelController.PlayerEntities.Contains(e))
                _levelController.PlayerEntities.Add(e);    
        }
        
        private void UnregisterDeletedEntity(Entity e)
        {
            if (e.owner == Entity.Owner.AI && _levelController.AIEntities.Contains(e))
                _levelController.AIEntities.Remove(e);
            else if (e.owner == Entity.Owner.Player && _levelController.PlayerEntities.Contains(e))
                _levelController.PlayerEntities.Remove(e);
                
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