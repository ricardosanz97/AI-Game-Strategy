using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using Pathfinding;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    public SpawnablesManager SpawnablesManager;
    
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle().WithArguments(2);
    }
}
