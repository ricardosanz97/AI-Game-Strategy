
using System.ComponentModel;
using Zenject;
using AI.StrategicAI;
using InfluenceMap;
using UnityEngine;

namespace DependenciesInstallers
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public StrategicObjectives AIObjectives;
        public InfluenceMapComponent InfluenceMapComponentPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<HighLevelAI>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            //cada ia tiene su propio gestor de recursos
            Container.Bind<AIResourcesAllocator>().FromNew().AsTransient();
            //cada ia tiene su propio modulo de analisis
            Container.Bind<AiAnalyzer>().FromNew().AsTransient().WithArguments(AIObjectives);
            Container.Bind<TurnHandler>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<InfluenceMapComponent>().FromComponentInNewPrefab(InfluenceMapComponentPrefab).AsSingle();
        }
    }
}