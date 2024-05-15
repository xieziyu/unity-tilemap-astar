using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZYTools
{
    public class Pathfinder
    {
        #region Readonly Constants
        private static readonly int StraightFactor = 10;
        private static readonly int DiagonalFactor = 14;
        private static readonly Vector3Int[] dirs = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right
        };
        private static readonly Vector3Int[] diagDirs = new Vector3Int[]
        {
            Vector3Int.up + Vector3Int.left,
            Vector3Int.up + Vector3Int.right,
            Vector3Int.down + Vector3Int.left,
            Vector3Int.down + Vector3Int.right
        };
        #endregion

        private List<PathNode> openList;
        private HashSet<Vector3Int> closedSet;

        private ISearchGrid grid;
        private bool allowDiagonals;
        private PathNode startNode;
        private PathNode endNode;
        private PathNode currentNode;

        public Pathfinder(ISearchGrid grid, bool allowDiagonals)
        {
            this.grid = grid;
            this.allowDiagonals = allowDiagonals;
            openList = new();
            closedSet = new();
        }

        #region Step Methods
        public void SetStartNode(PathNode startNode)
        {
            this.startNode = startNode;
        }

        public void SetEndNode(PathNode endNode)
        {
            this.endNode = endNode;
        }

        public PathfinderState GetState()
        {
            return new PathfinderState
            {
                currentNode = currentNode,
                startNode = startNode,
                endNode = endNode,
                openList = openList,
                closedSet = closedSet
            };
        }

        public List<PathNode> FindPath()
        {
            openList.Clear();
            closedSet.Clear();

            if (startNode == null || endNode == null || startNode == endNode)
            {
                return new();
            }

            openList.Add(startNode);
            while (openList.Count > 0)
            {
                openList.Sort();
                UpdateNextNode();
                if (openList.Contains(endNode))
                {
                    endNode = openList.Find(node => node.Equals(endNode));
                    break;
                }
            }
            return GetFinalPath();
        }

        public void StepStartFindPath()
        {
            openList.Clear();
            closedSet.Clear();
            openList.Add(startNode);
            currentNode = null;
        }

        public bool StepNext()
        {
            if (openList.Count > 0)
            {
                openList.Sort();
                UpdateNextNode();
                // debugger?.DrawState(currentNode, startNode, endNode, openList, closedSet);
                if (openList.Contains(endNode))
                {
                    endNode = openList.Find(node => node.Equals(endNode));
                    return true;
                }

                return false;
            }
            return true;
        }
        #endregion

        public void ResetPaths()
        {
            currentNode = null;
            openList.Clear();
            closedSet.Clear();
        }

        public List<PathNode> GetFinalPath()
        {
            Stack<PathNode> final = new();
            if (endNode == null)
            {
                return new();
            }
            PathNode nodeRef = endNode;
            while (nodeRef != null)
            {
                if (nodeRef.GetParent() != null)
                {
                    final.Push(nodeRef);
                }
                nodeRef = nodeRef.GetParent();
            }
            return final.ToList();
        }

        public List<PathNode> FindPath(PathNode startNode, PathNode endNode)
        {
            SetStartNode(startNode);
            SetEndNode(endNode);
            return FindPath();
        }

        public PathNode GetStartNode()
        {
            return startNode;
        }

        public PathNode GetEndNode()
        {
            return endNode;
        }

        private void UpdateNextNode()
        {
            currentNode = openList[0];
            openList.RemoveAt(0);
            closedSet.Add(currentNode.GetGridPos());

            List<PathNode> neighbors = FindNeighbors(currentNode, endNode);
            foreach (PathNode neighbor in neighbors)
            {
                CalculateCosts(neighbor, endNode);
                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        private void CalculateCosts(PathNode currentNode, PathNode endNode)
        {
            // H
            currentNode.SetH(DiagonalCost(currentNode.GetGridPos(), endNode.GetGridPos()));

            // G
            int dG = currentNode.GetParent() != null ? currentNode.GetParent().GetG() : 0;
            currentNode.SetG(dG + StraightFactor);
        }

        private List<PathNode> FindNeighbors(PathNode currentNode, PathNode endNode)
        {
            List<PathNode> neighbors = new();
            Vector3Int currentPos = currentNode.GetGridPos();

            // straight
            foreach (Vector3Int dir in dirs)
            {
                var posDir = currentPos + dir;
                if (
                    (endNode.GetGridPos() == posDir || grid.CanWalk(currentPos, posDir))
                    && !closedSet.Contains(posDir)
                )
                {
                    PathNode node = new(posDir);
                    node.SetParent(currentNode);
                    neighbors.Add(node);
                }
            }

            // diagonal
            if (allowDiagonals)
            {
                foreach (Vector3Int dir in diagDirs)
                {
                    var posDir = currentPos + dir;
                    if (
                        (endNode.GetGridPos() == posDir || grid.CanWalk(currentPos, posDir))
                        && !closedSet.Contains(posDir)
                    )
                    {
                        PathNode node = new(posDir);
                        node.SetParent(currentNode);
                        neighbors.Add(node);
                    }
                }
            }

            return neighbors;
        }

        private static int DiagonalCost(Vector3Int a, Vector3Int b)
        {
            int dx = Math.Abs(a.x - b.x);
            int dy = Math.Abs(a.y - b.y);
            int diag = Math.Min(dx, dy);
            return diag * DiagonalFactor + Math.Abs(dx - dy) * StraightFactor;
        }
    }
}
