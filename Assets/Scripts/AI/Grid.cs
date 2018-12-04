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
        private readonly List<Originator> _originators = new List<Originator>();
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
            Vector3 worldPos = new Vector3((-1 * x / 2 + i) * spacing, 0, (-1 * y / 2 + j) * spacing);
            Node temp = new Node();
            temp.Influence = 0;
            temp.Neighbours = new List<Node>();
            temp.WorldGameObject = Object.Instantiate(gridGameObject, worldPos, Quaternion.identity);
            temp.WorldPosition = worldPos;
            temp.WorldGameObject.GetComponent<InfluencePosition>().GridPositions = new[] {i, j};
            _grid[i, j] = temp;
        }

        public void UpdateMap()
        {
            _updateTimer -= Time.deltaTime;
            
            if(_updateTimer > 0)
                return;

            _updateTimer = 0.5f;
            
            ClearInfluenceData();
        }

        private void ClearInfluenceData()
        {
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    _grid[i, j].Influence = 0;
                    _grid[i, j].Color = Color.black;
                    if (_grid[i, j].WorldGameObject.GetComponent<Renderer>().enabled)
                    {
                        _grid[i, j].WorldGameObject.GetComponent<Renderer>().material.color = Color.black;

                        _grid[i, j].WorldGameObject.GetComponent<Renderer>().enabled = false;
                    }
                }
            }
        }
    }
}