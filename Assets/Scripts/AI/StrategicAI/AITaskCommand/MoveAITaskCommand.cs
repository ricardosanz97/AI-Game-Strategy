using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrategicAI;

public class MoveAITaskCommand : AITaskCommand {

    private Troop troopToMove;

    public MoveAITaskCommand(AbstractNPCBrain brain)
    {
        this.troopToMove = brain.GetComponent<Troop>();
    }

    public override void PerformCommand()
    {
        troopToMove.GetComponent<MoveOrder>().Move = true;//ahora estamos en estado move
        CustomPathfinding.Node objective = troopToMove.GetComponent<Troop>().possibleMovements[Random.Range(0, troopToMove.GetComponent<Troop>().possibleMovements.Count)];
        troopToMove.GetComponent<Move>().PathReceived = true;
        troopToMove.GetComponent<Move>().OnGoingCell = objective.cell;

    }

}
