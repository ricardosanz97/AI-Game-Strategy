using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfluenceMap
{
    public class Node
    {
        public Vector3 WorldPosition;
        public List<KeyValuePair<InfluenceType,float>> Influences = new List<KeyValuePair<InfluenceType, float>>();
        public List<Node> Neighbours;
        public Color Color;
        public GameObject WorldGameObject;
        
        public bool HasInfluenceOfType(InfluenceType type)
        {
            foreach (var influence in Influences)
            {
                if (influence.Key == type)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Get the influence of a given type if exists
        /// </summary>
        /// <param name="type">The influence type</param>
        /// <returns>The value of the influence given the type or -1 if the type doesn't exists</returns>
        public KeyValuePair<InfluenceType, float> GetInfluenceOfType(InfluenceType type)
        {
            foreach (var influence in Influences)
            {
                if(influence.Key == type)
                    return new KeyValuePair<InfluenceType, float>(influence.Key,influence.Value);
            }
            
            return new KeyValuePair<InfluenceType, float>(0,-1);
        }
    }

}
