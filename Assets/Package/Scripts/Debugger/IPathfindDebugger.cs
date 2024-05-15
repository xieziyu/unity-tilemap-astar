using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
    public interface IPathfindDebugger
    {
        public void Clear();
        public void DrawStartNode(PathNode startNode);
        public void DrawEndNode(PathNode endNode);
        public void DrawCurrentNode(PathNode currentNode);
        public void DrawStepNode(PathNode stepNode);
        public void DrawOpenList(IEnumerable<PathNode> openList);
        public void DrawClosedSet(ISet<Vector3Int> closedSet);
        public void DrawState(PathfinderState state);
    }

    public enum PathfindDebugNodeType
    {
        None,
        CurrentNode,
        StartNode,
        EndNode,
        OpenNode,
        ClosedNode,
        StepNode
    }
}
