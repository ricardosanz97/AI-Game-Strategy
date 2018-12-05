using System.Collections.Generic;
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
            influenceGrid.CreateMap(X, Y, Spacing, InfluenceNodePredab, true);
            influenceGrid.InfluenceMask = InfluenceMask;
            InfluenceMapTexture.texture = influenceGrid.InfluenceMapTexture;

            var influencers = FindObjectsOfType<Influencer>();

            foreach (var influencer in influencers)
            {
                influenceGrid.RegisterOriginator(influencer.Originator);
                Originators.Add(influencer);
            }

            influenceGrid.UpdateMap();
        }

    }
}