using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ZYTools
{
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

        [SerializeField]
        private TextMeshPro fCost;

        [SerializeField]
        private TextMeshPro gCost;

        [SerializeField]
        private TextMeshPro hCost;

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

            if (pathNode != null)
            {
                fCost.text = pathNode.GetF().ToString();
                gCost.text = pathNode.GetG().ToString();
                hCost.text = pathNode.GetH().ToString();
            }
        }

        public PathfindDebugNodeType GetNodeType()
        {
            return nodeType;
        }
    }
}
