using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StateInfo {

    public static void ExecuteOnEnter(STATE stateName, TROOP npc)
    {
        switch (npc)
        {
            case TROOP.Minion:

                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackMinion();
                        break;
                    case STATE.Move:
                        EnterMoveMinion();
                        break;
                    case STATE.Idle:
                        EnterIdleMinion();
                        break;
                    case STATE.None:
                        EnterDefaultMinion();
                        break;
                }
                break;

            case TROOP.Archer:
                switch (stateName)
                {
                    case STATE.Attack:
                        EnterAttackArcher();
                        break;
                    case STATE.Move:
                        EnterMoveArcher();
                        break;
                    case STATE.Idle:
                        EnterIdleArcher();
                        break;
                    case STATE.None:
                        EnterDefaultArcher();
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
            case TROOP.Minion:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackMinion();
                        break;
                    case STATE.Move:
                        ExitMoveMinion();
                        break;
                    case STATE.Idle:
                        ExitIdleMinion();
                        break;
                }
                break;

            case TROOP.Archer:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackArcher();
                        break;
                    case STATE.Move:
                        ExitMoveArcher();
                        break;
                    case STATE.Idle:
                        ExitMoveArcher();
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

    private static void EnterAttackMinion()
    {
    }

    private static void EnterMoveMinion()
    {
    }

    private static void EnterIdleMinion()
    {
    }

    private static void EnterDefaultMinion()
    {
    }

    private static void EnterAttackArcher()
    {
    }

    private static void EnterMoveArcher()
    {
    }

    private static void EnterIdleArcher()
    {
    }

    private static void EnterDefaultArcher()
    {
    }



    private static void ExitAttackMinion()
    {
    }

    private static void ExitMoveMinion()
    {
    }

    private static void ExitIdleMinion()
    {
    }

    private static void ExitAttackArcher()
    {
    }

    private static void ExitMoveArcher()
    {
        Debug.Log("ExitMoveArcher()");
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
        Debug.Log("ExitMoveDefault");
    }

    private static void ExitIdleDefault()
    {
        Debug.Log("ExitIdleDefault()");
    }

    private static void ExitDefault()
    {
    }
}
