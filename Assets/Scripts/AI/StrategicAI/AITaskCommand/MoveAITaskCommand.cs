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
        private LevelController _levelController;

        public MoveAITaskCommand(AbstractNPCBrain brain)
        {
            _levelController = Object.FindObjectOfType<LevelController>();
            this.troopToMove = brain.GetComponent<Troop>();
        }

        public override void PerformCommand()
        {
            //ahora estamos en estado move
            troopToMove.GetComponent<MoveOrder>().Move = true;
            troopToMove.StartCoroutine(PerformMovement());
        }

        IEnumerator PerformMovement()
        {
            yield return new WaitForSeconds(0.5f);

            var minInfluenceNode = GetMinInfluenceNode();
            
            _levelController.chosenNodesToMoveIA.Add(minInfluenceNode);
            

            troopToMove.GetComponent<Move>().PathReceived = true;
            
            Debug.Assert(minInfluenceNode != null, nameof(minInfluenceNode) + " != null");
            troopToMove.GetComponent<Move>().OnGoingCell = minInfluenceNode.cell;
        }

        private Node GetMinInfluenceNode()
        {
            InfluenceMapComponent influenceMapComponent = Object.FindObjectOfType<InfluenceMapComponent>();
            float minInfluence = int.MaxValue;
            Node minInfluenceNode = null;
            List<Node> sameInfluencesNode = new List<Node>();
            foreach (var possibleMovement in troopToMove.GetComponent<Troop>().possibleMovements)
            {
                float tempInfluence = influenceMapComponent.GetNodeAtLocation(possibleMovement.WorldPosition)
                    .GetTotalInfluenceAtNode();

                //intentamos movernos al de menor influencia y que no lo haya elegido otro.
                if (tempInfluence < minInfluence && !(_levelController.chosenNodesToMoveIA.Contains(possibleMovement)))
                {
                    minInfluenceNode = possibleMovement;
                    minInfluence = tempInfluence;
                }
                else if(tempInfluence == minInfluence && !(_levelController.chosenNodesToMoveIA.Contains(possibleMovement)))
                {
                      sameInfluencesNode.Add(possibleMovement);
                }
            }
            
            if(sameInfluencesNode.Count > 0)
                return sameInfluencesNode[Random.Range(0,sameInfluencesNode.Count)];
    
            return minInfluenceNode;
        }
    }
}

