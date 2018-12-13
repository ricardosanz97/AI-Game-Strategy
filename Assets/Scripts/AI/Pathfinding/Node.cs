using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace CustomPathfinding
{
    [RequireComponent(typeof(Collider),typeof(MeshRenderer))]
    public class Node : MonoBehaviour, IEquatable<Node>
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
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (GridX * 397) ^ GridZ;
            }
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


    }
}
