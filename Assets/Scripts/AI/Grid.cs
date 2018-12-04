using System.Collections.Generic;
using UnityEngine;

namespace InfluenceMap
{
    public class Grid
    {
        public LayerMask InfluenceMask;
        public Material testMat;
        public Material TestMat2;
        public Material StartingMat;
        public Texture2D InfluenceMapTexture;

        private Node[,] _grid;
        private bool _renderGroundGrid;
        private List<Originator> _originators = new List<Originator>();
        private float _updateTimer;

        public void RegisterOriginator(Originator originator)
        {
            
        }

        public void CreateMap(int x, int y, float spacing, GameObject gridGameObject, bool hasToRenderGround)
        {
            _renderGroundGrid = hasToRenderGround;
            InfluenceMapTexture = new Texture2D(x,y);
            _updateTimer = .5f;
            _grid = new Node[x,y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Node temp = new Node();
                    temp.Influence = 0;
                    temp.Neighbours = new List<Node>();
                    //instantiate the worldgameobject
                    //assign world position
                    //get component inlfuence position
                    
                }
            }
        }

        public void UpdateMap()
        {
            
        }

    }
}