using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
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

    public class PathfindDebugNode : MonoBehaviour
    {
        [SerializeField]
        private Transform currentNode;

        [SerializeField]
        private Transform startNode;

        [SerializeField]
        private Transform endNode;

        [SerializeField]
        private Transform openNode;

        [SerializeField]
        private Transform closedNode;

        [SerializeField]
        private Transform stepNode;

        private PathfindDebugNodeType nodeType;

        public void UpdateState(PathNode pathNode, PathfindDebugNodeType nodeType)
        {
            this.nodeType = nodeType;
            if (currentNode != null)
            {
                currentNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.CurrentNode);
            }
            if (startNode != null)
            {
                startNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.StartNode);
            }
            if (endNode != null)
            {
                endNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.EndNode);
            }
            if (openNode != null)
            {
                openNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.OpenNode);
            }
            if (closedNode != null)
            {
                closedNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.ClosedNode);
            }
            if (stepNode != null)
            {
                stepNode.gameObject.SetActive(nodeType == PathfindDebugNodeType.StepNode);
            }
        }

        public PathfindDebugNodeType GetNodeType()
        {
            return nodeType;
        }
    }
}
