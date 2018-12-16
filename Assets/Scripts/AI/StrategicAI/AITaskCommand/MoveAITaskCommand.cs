using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StrategicAI;
using DG.Tweening;

public class MoveAITaskCommand : AITaskCommand {

    private Troop troopToMove;

    public MoveAITaskCommand(AbstractNPCBrain brain)
    {
        this.troopToMove = brain.GetComponent<Troop>();
    }

    public override void PerformCommand()
    {
        troopToMove.GetComponent<MoveOrder>().Move = true;//ahora estamos en estado move
        troopToMove.StartCoroutine(PerformMovement());
    }

    IEnumerator PerformMovement()
    {
        yield return new WaitForSeconds(0.5f);
        int random = Random.Range(0, troopToMove.GetComponent<Troop>().possibleMovements.Count);
        CustomPathfinding.Node objective = troopToMove.GetComponent<Troop>().possibleMovements[random];
        troopToMove.GetComponent<Move>().PathReceived = true;
        troopToMove.GetComponent<Move>().OnGoingCell = objective.cell;
    }

}
