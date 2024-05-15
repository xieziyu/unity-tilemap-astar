using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
    public struct PathfinderState
    {
        public PathNode startNode;
        public PathNode endNode;
        public List<PathNode> openList;
        public HashSet<Vector3Int> closedSet;
        public PathNode currentNode;
    }
}
