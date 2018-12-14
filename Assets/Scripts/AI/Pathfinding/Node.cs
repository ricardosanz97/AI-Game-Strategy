using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace CustomPathfinding
{
    [RequireComponent(typeof(Collider),typeof(MeshRenderer))]
    public class Node : MonoBehaviour
    {
        public enum ENodeType
        {
            Walkable, NonWalkable, Invisible, NextToWall
        }
        private int _cost;
        public Vector3 WorldPosition { get; set; }
        public int GridX { get; set; }
        public int GridZ { get; set; }
        public ENodeType NodeType { get; set; }
        public float NodeRadius;
        public Color initialColor;
        public CellBehaviour cell;
        public PathfindingGrid pathfindingGrid;

        private void Start()
        {
            initialColor = this.GetComponent<MeshRenderer>().material.color;
        }

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public bool Equals(Node other)
        {
            Debug.Assert(other != null, nameof(other) + " != null");
            return GridX == other.GridX && GridZ == other.GridZ;
        }
        
        public CellBehaviour GetOurCell()
        {
            return cell;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (GridX * 397) ^ GridZ;
            }
        }

        public Node FindNodeFromThis(int x, int z)
        {
            int currentNodeX = GridX;
            int currentNodeZ = GridZ;

            int returnNodeX = currentNodeX + x;
            int returnNodeZ = currentNodeZ + z;

            Node node = GetNodeAt(returnNodeX, returnNodeZ);
            if (node != null)
            {
                return node;
            }
            return null;
        }

        public Node GetNodeAt(int x, int z)
        {
            print("Intentando obtener el nodo en " + x + ", " + z);
            
            if (x < 0 || z < 0 || x >= pathfindingGrid.GridWorldSize.x || z >= pathfindingGrid.GridWorldSize.y)
            {
                return null;
            }

            if (pathfindingGrid.Grid[x,z] != null)
            {
                return pathfindingGrid.Grid[x, z];
            }
            return null;
        }

        public void ColorAsPossibleMovementDistance()
        {
            this.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        public void ResetColor()
        {
            this.GetComponent<MeshRenderer>().material.color = initialColor;
        }

        public void ColorAsPossibleAttackDistance()
        {
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
        }

        public void ColorAsPossibleAttack()
        {
            this.GetComponent<MeshRenderer>().material.color = Color.black;
        }

        public void ColorAsPossibleTurretExplosion()
        {
            this.GetComponent<MeshRenderer>().material.color = Color.cyan;
        }
    
    }
}
