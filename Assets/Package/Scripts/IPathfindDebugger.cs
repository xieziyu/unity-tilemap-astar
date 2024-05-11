using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
    public interface IPathfindDebugger
    {
        public void Init();

        public void DrawState(
            PathNode currentNode,
            PathNode startNode,
            PathNode endNode,
            List<PathNode> openList,
            HashSet<Vector3Int> closedSet
        );
    }
}
