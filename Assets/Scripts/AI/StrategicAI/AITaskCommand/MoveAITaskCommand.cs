using System.Collections;
using System.Collections.Generic;
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
            troopToMove.GetComponent<MoveOrder>().Move = true;//ahora estamos en estado move
            troopToMove.StartCoroutine(PerformMovement());
        }

        IEnumerator PerformMovement()
        {
            yield return new WaitForSeconds(0.5f);

            var minInfluenceNode = GetMinInfluenceNode();

            troopToMove.GetComponent<Move>().PathReceived = true;
            
            Debug.Assert(minInfluenceNode != null, nameof(minInfluenceNode) + " != null");
            troopToMove.GetComponent<Move>().OnGoingCell = minInfluenceNode.cell;
        }

        private Node GetMinInfluenceNode()
        {
            InfluenceMapComponent influenceMapComponent = Object.FindObjectOfType<InfluenceMapComponent>();
            float minInfluence = int.MaxValue;
            Node minInfluenceNode = null;

            foreach (var possibleMovement in troopToMove.GetComponent<Troop>().possibleMovements)
            {
                float tempInfluence = influenceMapComponent.GetNodeAtLocation(possibleMovement.WorldPosition)
                    .GetTotalInfluenceAtNode();

                if (tempInfluence < minInfluence)
                {
                    minInfluenceNode = possibleMovement;
                    minInfluence = tempInfluence;
                }
            }

            return minInfluenceNode;
        }
    }
}

