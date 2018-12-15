
using System.ComponentModel;
using Zenject;
using InfluenceMap;
using Pathfinding;
using StrategicAI;
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
            Container.Bind<TasksAllocator>().FromNew().AsSingle();
            Container.Bind<GameBoardAnalyzer>().FromNew().AsSingle();
            Container.Bind<TurnHandler>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<HighLevelAI>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            Container.Bind<InfluenceMapComponent>().FromComponentInNewPrefab(influenceMapPrefab).AsSingle();
            Container.Bind<BloodController>().FromNewComponentOn(this.gameObject).AsSingle();
            Container.Bind<SpawnablesManager>().FromNewComponentOn(this.gameObject).AsSingle();
            
            //Container.Bind<SpawnablesManager>().FromComponentInNewPrefab(SpawnablesManager).AsSingle();
            //Container.Bind<Pathfinding.PathfindingManager>().FromComponentInNewPrefab(PathfindingManagerPrefab).AsSingle();
        }
    }
}