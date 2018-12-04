using System;
using UnityEngine;

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

        public int Cost
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public bool Equals(Node other)
        {
            return GridX == other.GridX && GridZ == other.GridZ;
        }
        

        public override int GetHashCode()
        {
            unchecked
            {
                return (GridX * 397) ^ GridZ;
            }
        }
    
    }
}
