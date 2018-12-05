using System.Collections.Generic;
using UnityEngine;

namespace InfluenceMap
{
    public class Grid
    {
        public LayerMask InfluenceMask;
        public Material TestMat;
        public Material TestMat2;
        public Material StartingMat;
        public Texture2D InfluenceMapTexture;

        private Node[,] _grid;
        private bool _renderGroundGrid;
        private List<Originator> _originators = new List<Originator>();
        private float _updateTimer;

        public void RegisterOriginator(Originator originator)
        {
            _originators.Add(originator);
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
                    InitializeInfluenceNode(x, y, spacing, gridGameObject, i, j);
                }
            }
            
            FillNeighbours(x, y);
        }
        
        public void UpdateMap()
        {
            _updateTimer -= Time.deltaTime;
            
            if(_updateTimer > 0)
                return;

            _updateTimer = 0.5f;
            
            ClearInfluenceData();

            UpdateInfluences();
            
            ApplyToTexture();
        }


        #region HelperMethods
        
        private void FillNeighbours(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (i % x == 0) //Column on the far left
                    {
                        _grid[i, j].Neighbours.Add(_grid[i + 1, j]);
                        if (j % y == 0) //Bottom Left Corner
                            _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        else if (j % y == y - 1) //Top Left Corner
                            _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                        else
                        {
                            _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                            _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        }
                    }
                    else if (i % x == x - 1) //Column on the far right
                    {
                        _grid[i, j].Neighbours.Add(_grid[i - 1, j]);
                        if (j % y == 0) //Bottom Left Corner
                            _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        else if (j % y == y - 1)
                            _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                        else
                        {
                            _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                            _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        }
                    }
                    else if (j % y == 0) //Bottom Row (sides excluded)
                    {
                        _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        _grid[i, j].Neighbours.Add(_grid[i + 1, j]);
                        _grid[i, j].Neighbours.Add(_grid[i - 1, j]);
                        //m_Grid[i, j+1].worldObject.GetComponent<Renderer>().material = testMat;
                    }
                    else if (j % y == y - 1) //Top Row (sides excluded)
                    {
                        _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                        _grid[i, j].Neighbours.Add(_grid[i + 1, j]);
                        _grid[i, j].Neighbours.Add(_grid[i - 1, j]);
                    }
                    else //Middle only
                    {
                        _grid[i, j].Neighbours.Add(_grid[i, j - 1]);
                        _grid[i, j].Neighbours.Add(_grid[i, j + 1]);
                        _grid[i, j].Neighbours.Add(_grid[i + 1, j]);
                        _grid[i, j].Neighbours.Add(_grid[i - 1, j]);
                    }
                }
            }
        }

        private void InitializeInfluenceNode(int x, int y, float spacing, GameObject gridGameObject, int i, int j)
        {
            Vector3 worldPos = Vector3.zero + new Vector3(spacing / 2, 0, spacing / 2) + Vector3.right * i +
                               Vector3.forward * j;
            Node temp = new Node();
            temp.Influences.Clear();
            temp.Neighbours = new List<Node>();
            temp.WorldGameObject = Object.Instantiate(gridGameObject, worldPos, Quaternion.identity);
            temp.WorldPosition = worldPos;
            temp.WorldGameObject.GetComponent<InfluencePosition>().GridPositions = new[] {i, j};
            _grid[i, j] = temp;
        }

        private void UpdateInfluences()
        {
            for (int i = 0; i < _originators.Count; i++)
            {
                Collider[] influencePositions =
                    Physics.OverlapSphere(_originators[i].WorldPosition, _originators[i].InfluenceRange, InfluenceMask);

                for (int j = 0; j < influencePositions.Length; j++)
                {
                    int indexI = influencePositions[j].GetComponent<InfluencePosition>().GridPositions[0];
                    int indexJ = influencePositions[j].GetComponent<InfluencePosition>().GridPositions[1];

                    //get the reference to the grid based on the position in the world
                    Node node = _grid[indexI, indexJ];

                    if (!node.HasInfluenceOfType(_originators[i].InfluenceType))
                    {
                        AssignInfluenceAtNode(i, influencePositions, j, node);
                    }
                    else
                    {
                        UpdateInfluenceAtNode(node, i, influencePositions, j);
                    }

                    SetMaterialAndDraw(node, i);
                }
            }
        }

        private void SetMaterialAndDraw(Node node, int i)
        {
            for (int k = 0; k < node.Influences.Count; k++)
            {
                if (_renderGroundGrid)
                {
                    node.WorldGameObject.GetComponent<Renderer>().enabled = true;
                    node.WorldGameObject.GetComponent<Renderer>().material.color +=
                        ((Color) _originators[i].Color) * (node.Influences[k].Value) *
                        (_originators[i].Influence);
                }

                node.Color += ((Color) _originators[i].Color) * (node.Influences[k].Value) * (_originators[i].Influence);
                if (!_renderGroundGrid)
                    node.WorldGameObject.GetComponent<Renderer>().enabled = false;
            }
        }

        private void ApplyToTexture()
        {
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    _grid[i, j].Color.a = 255;
                    InfluenceMapTexture.SetPixel(i, j, _grid[i, j].Color);
                }
            }

            InfluenceMapTexture.Apply();
        }

        private void AssignInfluenceAtNode(int i, Collider[] influencePositions, int j, Node node)
        {
//calculate the influence per node in range
            KeyValuePair<InfluenceType, float> calculatedInfluence = new KeyValuePair<InfluenceType, float>(
                _originators[i].InfluenceType,
                (Vector3.Distance(_originators[i].WorldPosition, influencePositions[j].transform.position)) /
                _originators[i].InfluenceRange
            );
            node.Influences.Add(calculatedInfluence);
        }

        private void UpdateInfluenceAtNode(Node node, int i, Collider[] influencePositions, int j)
        {
            KeyValuePair<InfluenceType, float> influenceAtNode =
                node.GetInfluenceOfType(_originators[i].InfluenceType);

            //calculate the influence per node in range
            KeyValuePair<InfluenceType, float> calculatedInfluence = new KeyValuePair<InfluenceType, float>(
                _originators[i].InfluenceType,
                influenceAtNode.Value +
                (Vector3.Distance(_originators[i].WorldPosition, influencePositions[j].transform.position)) /
                _originators[i].InfluenceRange
            );

            if (influenceAtNode.Value != -1)
                node.Influences[(int) influenceAtNode.Key] = calculatedInfluence;
        }

        private void ClearInfluenceData()
        {
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++) 
                {
                    _grid[i, j].Influences = new List<KeyValuePair<InfluenceType, float>>();
                    _grid[i, j].Color = Color.black;
                    if (_grid[i, j].WorldGameObject.GetComponent<Renderer>().enabled)
                    {
                        _grid[i, j].WorldGameObject.GetComponent<Renderer>().material.color = Color.black;

                        _grid[i, j].WorldGameObject.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
        
        #endregion
    }

        
}