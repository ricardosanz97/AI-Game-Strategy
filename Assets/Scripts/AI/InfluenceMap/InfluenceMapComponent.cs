using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace InfluenceMap
{
    public class InfluenceMapComponent : MonoBehaviour
    {
        public RawImage InfluenceMapTexture;
        public GameObject InfluenceNodePredab;
        public List<Influencer> Originators;
        public LayerMask InfluenceMask;
        public int X = 10;
        public int Y = 10;
        public float Spacing = 1.0f;

        private InfluenceMap.Grid influenceGrid;

        private void Start()
        {
            influenceGrid = new Grid();
            influenceGrid.CreateMap(X, Y, Spacing, InfluenceNodePredab, true, gameObject);
            influenceGrid.InfluenceMask = InfluenceMask;
            InfluenceMapTexture.texture = influenceGrid.InfluenceMapTexture;

            UpdateInfluenceMap();
            
            Entity.OnTroopSpawned += UpdateInfluenceMap;
        }

        private void UpdateInfluenceMap()
        {
            foreach (var influencer in FindObjectsOfType<Influencer>())
            {
                //si no se le asigna la posicion calculara la influencia desde el 0.
                influencer.Originator.WorldPosition = influencer.transform.position;
                influenceGrid.RegisterOriginator(influencer.Originator);
                Originators.Add(influencer);
            }
            
            influenceGrid.UpdateMap();
        }
    }
}