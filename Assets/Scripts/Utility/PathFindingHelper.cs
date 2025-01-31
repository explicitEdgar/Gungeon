using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class PathFindingHelper
    {   

        public static void FindPath<T>(NodeBase<T> startNode,NodeBase<T> targetNode,List<NodeBase<T>> pathNode)
        {
            var toSearch = ListPool<NodeBase<T>>.Get();
            toSearch.Add(startNode);
            var processed = ListPool<NodeBase<T>>.Get();

            while(toSearch.Count > 0)
            {
                var current = toSearch[0];
                foreach (var t in toSearch)
                {
                    if (t.F < current.F || Mathf.Abs(t.F - current.F) < 0.001f && t.H < current.H)
                    {
                        current = t;
                    }
                }

                processed.Add(current);
                toSearch.Remove(current);

                if(current == targetNode)
                {
                    var currentPathTile = targetNode;
                    var count = 100;
                    while(currentPathTile != startNode)
                    {
                        pathNode.Add(currentPathTile);
                        currentPathTile = currentPathTile.Connection;
                        count--;
                        if (count < 0) throw new Exception();
                    }

                    toSearch.Release2Pool();
                    processed.Release2Pool();
                    return;
                }

                foreach(var neighbor in current.Neighbors.Where(t => t.Walkable && !processed.Contains(t)))
                {
                    var inSearch = toSearch.Contains(neighbor);

                    var costToNeighbor = current.G + current.GetDistance(neighbor);

                    if(!inSearch || costToNeighbor < neighbor.G)
                    {
                        neighbor.G = costToNeighbor;
                        neighbor.Connection = current;

                        if(!inSearch)
                        {
                            neighbor.H = neighbor.GetDistance(targetNode);
                            toSearch.Add(neighbor);
                        }
                    }
                }
            }

            toSearch.Release2Pool();
            processed.Release2Pool();
        }

        //TÎªVecotr2µÈµÈ
        public interface ICoords<T>
        {
            float GetDistance(ICoords<T> other);

            T Pos { get; set; }
        }

        public class TileCoords : ICoords<Vector3Int>
        {
            public float GetDistance(ICoords<Vector3Int> other)
            {
                var distance = new Vector3Int(Mathf.Abs(Pos.x - other.Pos.x), Mathf.Abs(Pos.y - other.Pos.y));

                var lowest = Mathf.Min(distance.x, distance.y);
                var highest = Mathf.Max(distance.x, distance.y);

                var horizontalMovesRequired = highest - lowest;

                return lowest * 14 + horizontalMovesRequired * 10;
            }

            public Vector3Int Pos { get; set; }

        }

        public abstract class NodeBase<T>
        {
            public ICoords<T> Coords;

            public float GetDistance(NodeBase<T> other) => Coords.GetDistance(other.Coords);

            public bool Walkable { get; private set; }

            public virtual NodeBase<T> Init(bool walkable,ICoords<T> coords)
            {
                Walkable = walkable;
                Coords = coords;
                return this;
            }

            #region Pathfinding

            public List<NodeBase<T>> Neighbors { get; set; }
            public NodeBase<T> Connection { get; set; }

            public float G { get; set; }
            public float H { get; set; }
            public float F => G + H;

            public abstract void CatchNeighbors();

            #endregion
        }

        public class TileNode : NodeBase<Vector3Int>
        {
            private readonly DynaGrid<TileNode> mGrid;

            public TileNode(DynaGrid<TileNode> grid)
            {
                mGrid = grid;
            }

            static readonly List<Vector3Int> Directions = new List<Vector3Int>()
            {
                new Vector3Int(0,1,0),
                new Vector3Int(0,-1,0),
                new Vector3Int(1,0,0),
                new Vector3Int(-1,0,0),
            };

            public override void CatchNeighbors()
            {
                Neighbors = new List<NodeBase<Vector3Int>>();

                foreach(var direction in Directions)
                {
                    var pos = Coords.Pos + direction;
                    if (mGrid[pos.x,pos.y] != null)
                    {
                        Neighbors.Add(mGrid[pos.x, pos.y]);
                    }
                }
            }

        }

        
    }
}