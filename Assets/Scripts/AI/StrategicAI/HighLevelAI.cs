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
            _strategicObjective = GetOrAddComponent<AttackTroopsObjective>();
            
            //todo programar el arbol
            if (CalculateSetDamage(_levelController.AIEntities) >= CalculateSetDamage(_levelController.PlayerEntities))
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