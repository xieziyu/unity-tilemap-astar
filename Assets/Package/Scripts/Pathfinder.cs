using System;
using System.Collections.Generic;
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

        private List<PathNode> openList = new();
        private HashSet<Vector3Int> closedSet = new();

        private ISearchGrid grid;
        private bool allowDiagonals;

        public Pathfinder(ISearchGrid grid, bool allowDiagonals)
        {
            this.grid = grid;
            this.allowDiagonals = allowDiagonals;
        }

        public List<PathNode> FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            openList.Clear();
            closedSet.Clear();

            List<PathNode> final = new();
            if (startPos == endPos)
            {
                return final;
            }

            PathNode startNode = new PathNode(startPos);
            PathNode endNode = new PathNode(endPos);

            openList.Add(startNode);
            while (openList.Count > 0)
            {
                openList.Sort();

                PathNode currentNode = openList[0];
                openList.RemoveAt(0);
                closedSet.Add(currentNode.GetGridPos());

                List<PathNode> neighbors = FindNeighbors(currentNode);
                foreach (PathNode neighbor in neighbors)
                {
                    CalculateCosts(neighbor, endNode);
                    openList.Add(neighbor);
                }

                if (openList.Contains(endNode))
                {
                    endNode = openList.Find(node => node.Equals(endNode));
                    break;
                }
            }

            PathNode nodeRef = endNode.GetParent();
            while (nodeRef != null)
            {
                final.Add(nodeRef);
                nodeRef = nodeRef.GetParent();
            }
            return final;
        }

        private void CalculateCosts(PathNode currentNode, PathNode endNode)
        {
            // H
            currentNode.SetH(DiagonalCost(currentNode.GetGridPos(), endNode.GetGridPos()));

            // G
            int dG = currentNode.GetParent() != null ? currentNode.GetParent().GetG() : 0;
            currentNode.SetG(dG + StraightFactor);
        }

        private List<PathNode> FindNeighbors(PathNode currentNode)
        {
            List<PathNode> neighbors = new();
            Vector3Int currentPos = currentNode.GetGridPos();

            // straight
            foreach (Vector3Int dir in dirs)
            {
                var posDir = currentPos + dir;
                if (grid.CanWalk(currentPos, posDir) && !closedSet.Contains(posDir))
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
                    if (grid.CanWalk(currentPos, posDir) && !closedSet.Contains(posDir))
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
