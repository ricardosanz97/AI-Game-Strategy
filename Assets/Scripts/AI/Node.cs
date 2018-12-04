using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfluenceMap
{
    public class Node
    {
        public Vector3 WorldPosition;
        public float Influence;
        public List<Node> Neighbours;
        public Color Color;
        public GameObject WorldGameObject;
        
        // TODO support different types of influence
    }

}
