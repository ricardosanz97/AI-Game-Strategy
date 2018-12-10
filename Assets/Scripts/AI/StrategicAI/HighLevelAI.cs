using System.Collections.Generic;
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
        
        [Inject] [SerializeField] private AiAnalyzer _analyzer;
        [Inject] private AIResourcesAllocator _allocator;
        [Inject] private TurnHandler _turnHandler;
        public List<Entity> ControlledEntites;
        
        public IAPersonality CurrentIaPersonality { get; private set; }

        private void OnEnable()
        {
            SpawnablesManager.OnSpawnedTroop += RegisterControlledEntity;
        }

        private void OnDisable()
        {
            SpawnablesManager.OnSpawnedTroop -= RegisterControlledEntity;
        }

        private void Start()
        {
            CurrentIaPersonality = IAPersonality.Offensive;
            ControlledEntites = new List<Entity>();
        }

        public void EvaluateGameState()
        { 
            throw new System.NotImplementedException();
        }

        public void PlayTurn()
        {
            Debug.Log("Ia Player turn");
            _turnHandler.AIDone = true;
        }

        private void ChangePersonality(IAPersonality newPersonality)
        {
            //check they are different
            Assert.AreNotEqual(newPersonality,CurrentIaPersonality);
            
            CurrentIaPersonality = newPersonality;
            
            _analyzer.OnPersonalityChanged();
        }

        private void RegisterControlledEntity(Entity e)
        {
            if(e.owner == Entity.Owner.AI)
                ControlledEntites.Add(e);
        }


        public void OnResourcesAllocated()
        {
            //the assignments are done and now we have to iterate through each unity and depending on its task use a command or other
            
        }
    }
}