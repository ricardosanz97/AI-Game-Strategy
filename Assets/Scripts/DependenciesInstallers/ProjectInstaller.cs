using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller<ProjectInstaller> 
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().AsSingle().WithArguments(2);
    }
}
