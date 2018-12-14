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

            InitializeInfluenceMap();
            
            SpawnablesManager.OnSpawnedTroop += UpdateInfluenceMap;
        }

        public void InitializeInfluenceMap()
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
        
        public void UpdateInfluenceMap(Entity spawned)
        {
            Influencer influencer = spawned.GetComponent<Influencer>();
            
            if(influencer == null)
            {
                return;
            }
            
            influencer.Originator.WorldPosition = influencer.transform.position;
            influenceGrid.RegisterOriginator(influencer.Originator);
            Originators.Add(influencer);
            
            influenceGrid.UpdateMap();
        }

        public Node GetNodeAtLocation(Vector3 location)
        {
            float percentX = (location.x) / X;
            float percentY = (location.z) / Y;

            int x = Mathf.RoundToInt((X) * percentX);
            int y = Mathf.RoundToInt((Y-1) * percentY);
            return influenceGrid._grid[x,y];
        }
        
        public List<Node> GetKRingsOfNodes(Node currentNode, int k)
        {
            List<Node> nodeList = new List<Node>();
            
            for (int i = -k; i <= k; i++)
            {
                for (int j = -k; j <= k; j++)
                {
                    if (i == 0 && j == 0) continue;

                    var indexI = currentNode.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[0];
                    var indexJ = currentNode.WorldGameObject.GetComponent<InfluencePosition>().GridPositions[1];
                    
                    var x = indexI + i;
                    var z = indexJ + j;

                    if ((x >= 0 && x < X) && (z >= 0 && z < Y))
                    {
                        nodeList.Add(influenceGrid._grid[indexI + i, indexJ + j]);
                    }
                }
            }
            
            return nodeList;
        }
    }
}