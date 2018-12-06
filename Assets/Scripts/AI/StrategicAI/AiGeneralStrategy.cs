using UnityEngine;
using Zenject;

namespace AI.StrategicAI
{
    public class AiGeneralStrategy : MonoBehaviour
    {
        public StrategicObjectives StrategicObjectives;

        [Inject] private AiAnalyzer _analyzer;
        [Inject] private AiResourcesAllocator _allocator;

        public void EvaluateGameState()
        { 
            throw new System.NotImplementedException();
        }
    }
}