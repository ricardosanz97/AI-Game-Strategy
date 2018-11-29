using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller>
{
    public PathfindingManager PathfindingManagerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle().WithArguments(2);
        Container.Bind<PathfindingManager>().FromComponentInNewPrefab(PathfindingManagerPrefab).AsSingle();
    }
}
