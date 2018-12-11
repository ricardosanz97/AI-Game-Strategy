
using System.ComponentModel;
using Zenject;
using AI.StrategicAI;
using InfluenceMap;
using UnityEngine;

namespace DependenciesInstallers
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public InfluenceMapComponent influenceMapPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<AIResourcesAllocator>().FromNew().AsSingle();
            Container.Bind<AiAnalyzer>().FromNew().AsSingle();
            Container.Bind<TurnHandler>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<HighLevelAI>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            Container.Bind<InfluenceMapComponent>().FromComponentInNewPrefab(influenceMapPrefab).AsSingle();
            Container.Bind<BloodIndicatorController>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<SpawnablesManager>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<Pathfinding.PathfindingManager>().FromNewComponentOn(this.gameObject).AsSingle();
        }
    }
}