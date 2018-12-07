
using System.ComponentModel;
using Zenject;
using AI.StrategicAI;
using UnityEngine;

namespace DependenciesInstallers
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AiGeneralStrategy>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            Container.Bind<AiResourcesAllocator>().FromNew().AsSingle().NonLazy();
            Container.Bind<AiAnalyzer>().FromNew().AsSingle().NonLazy();
            Container.Bind<TurnHandler>().FromNewComponentOn(this.gameObject).AsSingle();
        }
    }
}