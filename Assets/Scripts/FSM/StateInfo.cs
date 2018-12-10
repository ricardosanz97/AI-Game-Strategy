using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateInfo {

    public static void ExecuteOnEnter(STATE stateName, TROOP npc)
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

    public static void ExecuteOnExit(STATE stateName, TROOP npc)
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

    private static void ExitIdleLauncher()
    {
        Debug.Log("ExitIdleLauncher()");
    }

    private static void EnterDefault()
    {
    }

    private static void EnterIdleDefault()
    {
    }

    private static void EnterMoveDefault()
    {
    }

    private static void EnterAttackDefault()
    {
    }

    private static void EnterDefaultTank()
    {
    }

    private static void EnterIdleTank()
    {
    }

    private static void EnterMoveTank()
    {
    }

    private static void EnterAttackTank()
    {
    }

    private static void EnterAttackPrisioner()
    {
    }

    private static void EnterMovePrisioner()
    {
    }

    private static void EnterIdlePrisioner()
    {
    }

    private static void EnterDefaultPrisioner()
    {
    }

    private static void EnterAttackLauncher()
    {
    }

    private static void EnterMoveLauncher()
    {
    }

    private static void EnterIdleLauncher()
    {
    }

    private static void EnterDefaultLauncher()
    {
    }



    private static void ExitAttackPrisioner()
    {
    }

    private static void ExitMovePrisioner()
    {
    }

    private static void ExitIdlePrisioner()
    {
    }

    private static void ExitAttackLauncher()
    {
    }

    private static void ExitMoveLauncher()
    {
        Debug.Log("ExitMoveLauncher()");
    }

    private static void ExitAttackTank()
    {
    }

    private static void ExitMoveTank()
    {
    }

    private static void ExitIdleTank()
    {
    }

    private static void ExitAttackDefault()
    {
    }

    private static void ExitMoveDefault()
    {
        Debug.Log("ExitMoveDefault()");
    }

    private static void ExitIdleDefault()
    {
        Debug.Log("ExitIdleDefault()");
    }

    private static void ExitDefault()
    {
    }
}
