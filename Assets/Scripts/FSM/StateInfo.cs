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
                    case STATE.MoveForward:
                        EnterMoveForwardMinion();
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
                    case STATE.MoveForward:
                        EnterMoveForwardArcher();
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
                    case STATE.MoveForward:
                        EnterMoveForwardTank();
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
                    case STATE.MoveForward:
                        EnterMoveForwardDefault();
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
                    case STATE.MoveForward:
                        ExitMoveForwardMinion();
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
                    case STATE.MoveForward:
                        ExitMoveForwardArcher();
                        break;
                    case STATE.Idle:
                        ExitMoveForwardArcher();
                        break;
                }
                break;

            case TROOP.Tank:
                switch (stateName)
                {
                    case STATE.Attack:
                        ExitAttackTank();
                        break;
                    case STATE.MoveForward:
                        ExitMoveForwardTank();
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
                    case STATE.MoveForward:
                        ExitMoveForwardDefault();
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
        throw new NotImplementedException();
    }

    private static void EnterIdleDefault()
    {
        throw new NotImplementedException();
    }

    private static void EnterMoveForwardDefault()
    {
        throw new NotImplementedException();
    }

    private static void EnterAttackDefault()
    {
        throw new NotImplementedException();
    }

    private static void EnterDefaultTank()
    {
        throw new NotImplementedException();
    }

    private static void EnterIdleTank()
    {
        throw new NotImplementedException();
    }

    private static void EnterMoveForwardTank()
    {
        throw new NotImplementedException();
    }

    private static void EnterAttackTank()
    {
        throw new NotImplementedException();
    }

    private static void EnterAttackMinion()
    {
        throw new NotImplementedException();
    }

    private static void EnterMoveForwardMinion()
    {
        throw new NotImplementedException();
    }

    private static void EnterIdleMinion()
    {
        throw new NotImplementedException();
    }

    private static void EnterDefaultMinion()
    {
        throw new NotImplementedException();
    }

    private static void EnterAttackArcher()
    {
        throw new NotImplementedException();
    }

    private static void EnterMoveForwardArcher()
    {
        throw new NotImplementedException();
    }

    private static void EnterIdleArcher()
    {
        throw new NotImplementedException();
    }

    private static void EnterDefaultArcher()
    {
        throw new NotImplementedException();
    }



    private static void ExitAttackMinion()
    {
        throw new NotImplementedException();
    }

    private static void ExitMoveForwardMinion()
    {
        throw new NotImplementedException();
    }

    private static void ExitIdleMinion()
    {
        throw new NotImplementedException();
    }

    private static void ExitAttackArcher()
    {
        throw new NotImplementedException();
    }

    private static void ExitMoveForwardArcher()
    {
        throw new NotImplementedException();
    }

    private static void ExitAttackTank()
    {
        throw new NotImplementedException();
    }

    private static void ExitMoveForwardTank()
    {
        throw new NotImplementedException();
    }

    private static void ExitIdleTank()
    {
        throw new NotImplementedException();
    }

    private static void ExitAttackDefault()
    {
        throw new NotImplementedException();
    }

    private static void ExitMoveForwardDefault()
    {
        throw new NotImplementedException();
    }

    private static void ExitIdleDefault()
    {
        throw new NotImplementedException();
    }

    private static void ExitDefault()
    {
        throw new NotImplementedException();
    }
}
