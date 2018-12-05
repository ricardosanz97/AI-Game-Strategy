using UnityEngine;

namespace InfluenceMap
{
    [System.Serializable]
    public class Originator
    {
        public Vector3 WorldPosition;
        public float Influence;
        public float InfluenceRange;
        public Color Color;
        public InfluenceType InfluenceType;
    }
}