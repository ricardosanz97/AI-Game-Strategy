
using System.ComponentModel;
using Zenject;
using AI.StrategicAI;
using InfluenceMap;
using Pathfinding;
using UnityEngine;

namespace DependenciesInstallers
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public InfluenceMapComponent influenceMapPrefab;
        
        public override void InstallBindings()
        {
            Container.Bind<PathfindingManager>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<LevelController>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<AIResourcesAllocator>().FromNew().AsNew();
            Container.Bind<AiAnalyzer>().FromNew().AsTransient().WithArguments(AIObjectives);
            Container.Bind<TurnHandler>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<HighLevelAI>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            Container.Bind<InfluenceMapComponent>().FromComponentInNewPrefab(influenceMapPrefab).AsSingle();
            Container.Bind<BloodIndicatorController>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<SpawnablesManager>().FromNewComponentOn(this.gameObject).AsSingle();
            
            //Container.Bind<SpawnablesManager>().FromComponentInNewPrefab(SpawnablesManager).AsSingle();
            //Container.Bind<Pathfinding.PathfindingManager>().FromComponentInNewPrefab(PathfindingManagerPrefab).AsSingle();
        }
    }
}