using System;
using UnityEngine;

namespace ZYTools
{
    public class PathNode : IComparable<PathNode>, IEquatable<PathNode>
    {
        private Vector3Int gridPos;
        private int g;
        private int h;
        private PathNode parent;

        public PathNode(Vector3Int gridPos)
        {
            this.gridPos = gridPos;
        }

        #region Implenetation of Interfaces
        public int CompareTo(PathNode other)
        {
            return GetF() - other.GetF();
        }

        public bool Equals(PathNode other)
        {
            return gridPos == other.gridPos;
        }
        #endregion

        #region Getters and Setters
        public Vector3Int GetGridPos()
        {
            return gridPos;
        }

        public int GetF()
        {
            return g + h;
        }

        public int GetG()
        {
            return g;
        }

        public void SetG(int g)
        {
            this.g = g;
        }

        public int GetH()
        {
            return h;
        }

        public void SetH(int h)
        {
            this.h = h;
        }

        public PathNode GetParent()
        {
            return parent;
        }

        public void SetParent(PathNode parent)
        {
            this.parent = parent;
        }
        #endregion
    }
}
