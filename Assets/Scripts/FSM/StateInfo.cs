using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using Zenject;

public class StateInfo : MonoBehaviour{
    
    [Inject]
    private Pathfinding.PathfindingManager _pathfindingManager;

    public void Start()
    {
        _pathfindingManager = FindObjectOfType<PathfindingManager>();
    }

    public void ExecuteOnEnter(STATE stateName, TROOP npc)
    {
        switch (npc)
        {
            case TROOP.Prisioner:

                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackPrisioner();
                        break;
                    case STATE.Move:
                        EnterMovePrisioner();
                        break;
                    case STATE.Idle:
                        EnterIdlePrisioner();
                        break;
                    case STATE.None:
                        EnterDefaultPrisioner();
                        break;
                }
                break;

            case TROOP.Launcher:
                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackLauncher();
                        break;
                    case STATE.Move:
                        EnterMoveLauncher();
                        break;
                    case STATE.Idle:
                        EnterIdleLauncher();
                        break;
                    case STATE.None:
                        EnterDefaultLauncher();
                        break;
                }
                break;

            case TROOP.Tank:
                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackTank();
                        break;
                    case STATE.Move:
                        EnterMoveTank();
                        break;
                    case STATE.Idle:
                        EnterIdleTank();
                        break;
                    case STATE.None:
                        EnterDefaultTank();
                        break;
                }
                break;
            case TROOP.None:
                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackDefault();
                        break;
                    case STATE.Move:
                        EnterMoveDefault();
                        break;
                    case STATE.Idle:
                        EnterIdleDefault();
                        break;
                    case STATE.None:
                        EnterDefault();
                        break;
                }
                break;
        }
    }

    public void ExecuteOnExit(STATE stateName, TROOP npc)
    {
        switch (npc)
        {
            case TROOP.Prisioner:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackPrisioner();
                        break;
                    case STATE.Move:
                        ExitMovePrisioner();
                        break;
                    case STATE.Idle:
                        ExitIdlePrisioner();
                        break;
                }
                break;

            case TROOP.Launcher:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackLauncher();
                        break;
                    case STATE.Move:
                        ExitMoveLauncher();
                        break;
                    case STATE.Idle:
                        ExitIdleLauncher();
                        break;
                }
                break;

            case TROOP.Tank:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackTank();
                        break;
                    case STATE.Move:
                        ExitMoveTank();
                        break;
                    case STATE.Idle:
                        ExitIdleTank();
                        break;
                }
                break;

            case TROOP.None:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackDefault();
                        break;
                    case STATE.Move:
                        ExitMoveDefault();
                        break;
                    case STATE.Idle:
                        ExitIdleDefault();
                        break;
                    case STATE.None:
                        ExitDefault();
                        break;
                }
                break;
        }
    }

    public void ExitIdleLauncher()
    {
        Debug.Log("ExitIdleLauncher()");
    }

    public void EnterDefault()
    {
    }

    public void EnterIdleDefault()
    {
    }

    public void EnterMoveDefault()
    {
    }

    public void EnterAttackDefault()
    {
    }

    public void EnterDefaultTank()
    {
    }

    public void EnterIdleTank()
    {
    }

    public void EnterMoveTank()
    {
    }

    public void EnterAttackTank()
    {
    }

    public void EnterAttackPrisioner()
    {
    }

    public void EnterMovePrisioner()
    {
    }

    public void EnterIdlePrisioner()
    {
    }

    public void EnterDefaultPrisioner()
    {
    }

    public void EnterAttackLauncher()
    {
    }

    public void EnterMoveLauncher()
    {
        
    }

    public void EnterIdleLauncher()
    {
    }

    public void EnterDefaultLauncher()
    {
    }



    public void ExitAttackPrisioner()
    {
    }

    public void ExitMovePrisioner()
    {
    }

    public void ExitIdlePrisioner()
    {
    }

    public void ExitAttackLauncher()
    {
    }

    public void ExitMoveLauncher()
    {
        if (_pathfindingManager != null)
        {
            Debug.Log("Pathfinding manager != null");
        }
        Debug.Log("ExitMoveLauncher()");
    }

    public void ExitAttackTank()
    {
    }

    public void ExitMoveTank()
    {
    }

    public void ExitIdleTank()
    {
    }

    public void ExitAttackDefault()
    {
    }

    public void ExitMoveDefault()
    {
        Debug.Log("ExitMoveDefault()");
    }

    public void ExitIdleDefault()
    {
        Debug.Log("ExitIdleDefault()");
    }

    public void ExitDefault()
    {
    }
}
