using System.Collections;
using System.Collections.Generic;
using CustomPathfinding;
using UnityEngine;
using StrategicAI;
using DG.Tweening;
using InfluenceMap;
using Debug = System.Diagnostics.Debug;
using Node = CustomPathfinding.Node;

namespace StrategicAI
{
    public class MoveAITaskCommand : AITaskCommand
    {

        private Troop troopToMove;

        public MoveAITaskCommand(AbstractNPCBrain brain)
        {
            this.troopToMove = brain.GetComponent<Troop>();
        }

        public override void PerformCommand()
        {
            //ahora estamos en estado move
            troopToMove.StartCoroutine(PerformMovement());
        }

        IEnumerator PerformMovement()
        {
            yield return new WaitForSeconds(0.5f);

            var minInfluenceNode = GetMinInfluenceNode();
            
            while(minInfluenceNode.NodeType == Node.ENodeType.NonWalkable)
            {
                minInfluenceNode = GetMinInfluenceNode();    
            }
            
            troopToMove.cell.PNode.NodeType = Node.ENodeType.Walkable;
            Object.FindObjectOfType<PathfindingGrid>().UpdateNode(troopToMove.cell.PNode);
            troopToMove.GetComponent<MoveOrder>().Move = true;
            troopToMove.GetComponent<Move>().PathReceived = true;
            
            Debug.Assert(minInfluenceNode != null, nameof(minInfluenceNode) + " != null");
            troopToMove.GetComponent<Move>().OnGoingCell = minInfluenceNode.cell;
            troopToMove.GetComponent<Move>().OnGoingCell.PNode.NodeType = Node.ENodeType.NonWalkable;
            Object.FindObjectOfType<PathfindingGrid>().UpdateNode(troopToMove.GetComponent<Move>().OnGoingCell.PNode);
        }

        private Node GetMinInfluenceNode()
        {
            InfluenceMapComponent influenceMapComponent = Object.FindObjectOfType<InfluenceMapComponent>();
            float minInfluence = int.MaxValue;
            Node minInfluenceNode = null;
            troopToMove.GetComponent<Troop>().GetCellsPossibleMovements();
            foreach (var possibleMovement in troopToMove.GetComponent<Troop>().possibleMovements)
            {
                float tempInfluence = influenceMapComponent.GetNodeAtLocation(possibleMovement.WorldPosition)
                    .GetTotalInfluenceAtNode();

                if (tempInfluence < minInfluence && possibleMovement.cell.PNode.NodeType != Node.ENodeType.NonWalkable)
                {
                    minInfluenceNode = possibleMovement;
                    minInfluence = tempInfluence;
                }
            }
            
            return minInfluenceNode;
        }
    }
}

