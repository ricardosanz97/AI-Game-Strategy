using UnityEngine;
using Zenject;

namespace AI.StrategicAI
{
    public class AiGeneralStrategy : MonoBehaviour
    {
        public StrategicObjectives StrategicObjectives;

        [Inject] private AiAnalyzer _analyzer;
        [Inject] private AiResourcesAllocator _allocator;
        [Inject] private TurnHandler _turnHandler;

        public void EvaluateGameState()
        { 
            throw new System.NotImplementedException();
        }

        public void PlayTurn()
        {
            Debug.Log("Ia Player turn");
            _turnHandler.AIDone = true;

        }
    }
}