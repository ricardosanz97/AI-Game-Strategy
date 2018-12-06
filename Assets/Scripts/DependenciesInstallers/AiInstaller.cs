
using System.ComponentModel;
using Zenject;
using AI.StrategicAI;
using UnityEngine;

namespace DependenciesInstallers
{
    public class AiInstaller : MonoInstaller<AiInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<AiGeneralStrategy>().FromComponentInHierarchy(true).AsSingle().NonLazy();
            Container.Bind<AiResourcesAllocator>().FromNew().AsSingle().NonLazy();
            Container.Bind<AiAnalyzer>().FromNew().AsSingle().NonLazy(); 
        }
    }
}