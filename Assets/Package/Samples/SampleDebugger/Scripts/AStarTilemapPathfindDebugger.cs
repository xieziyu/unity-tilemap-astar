using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
    public class AStarTilemapPathfindDebugger : MonoBehaviour, IPathfindDebugger
    {
        [SerializeField]
        private GameObject debugNodePrefab;

        [SerializeField]
        private AStarTilemap aStarTilemap;

        private Dictionary<Vector3Int, GameObject> nodeGOMap = new();
        private GameObject startNodeGO;
        private GameObject endNodeGO;

        private void Awake()
        {
            if (aStarTilemap != null)
            {
                aStarTilemap.SetDebugger(this);
            }
        }

        private void OnDestroy()
        {
            aStarTilemap = null;
            foreach (var go in nodeGOMap.Values)
            {
                Destroy(go);
            }
            nodeGOMap.Clear();
            nodeGOMap = null;
            startNodeGO = null;
            endNodeGO = null;
        }

        public void Clear()
        {
            foreach (var go in nodeGOMap.Values)
            {
                Destroy(go);
            }
            nodeGOMap.Clear();
            startNodeGO = null;
            endNodeGO = null;
        }

        #region Interface Implementation
        public void DrawStartNode(PathNode startNode)
        {
            if (startNode == null)
            {
                return;
            }
            var startPos = startNode.GetGridPos();
            var cellPosition = aStarTilemap.ToCellCenter(startPos);
            if (startNodeGO == null)
            {
                startNodeGO = Instantiate(debugNodePrefab, cellPosition, Quaternion.identity);
            }
            else
            {
                startNodeGO.transform.position = cellPosition;
            }
            UpdateNode(startNodeGO, startNode, startPos, PathfindDebugNodeType.StartNode);
        }

        public void DrawEndNode(PathNode endNode)
        {
            if (endNode == null)
            {
                return;
            }
            var endPos = endNode.GetGridPos();
            var cellPosition = aStarTilemap.ToCellCenter(endPos);
            if (endNodeGO == null)
            {
                endNodeGO = Instantiate(debugNodePrefab, cellPosition, Quaternion.identity);
            }
            else
            {
                endNodeGO.transform.position = cellPosition;
            }
            UpdateNode(endNodeGO, endNode, endPos, PathfindDebugNodeType.EndNode);
        }

        public void DrawCurrentNode(PathNode currentNode)
        {
            if (currentNode == null)
            {
                return;
            }
            var currentPos = currentNode.GetGridPos();
            if (!nodeGOMap.TryGetValue(currentPos, out GameObject currentGO))
            {
                currentGO = Instantiate(
                    debugNodePrefab,
                    aStarTilemap.ToCellCenter(currentPos),
                    Quaternion.identity
                );
            }
            UpdateNode(currentGO, currentNode, currentPos, PathfindDebugNodeType.CurrentNode);

            // Draw Parent Nodes
            var stepNode = currentNode.GetParent();
            while (stepNode != null)
            {
                DrawStepNode(stepNode);
                stepNode = stepNode.GetParent();
            }
        }

        public void DrawStepNode(PathNode stepNode)
        {
            if (stepNode == null)
            {
                return;
            }
            var stepPos = stepNode.GetGridPos();
            if (!nodeGOMap.TryGetValue(stepPos, out GameObject stepGO))
            {
                stepGO = Instantiate(
                    debugNodePrefab,
                    aStarTilemap.ToCellCenter(stepPos),
                    Quaternion.identity
                );
            }
            UpdateNode(stepGO, stepNode, stepPos, PathfindDebugNodeType.StepNode);
        }

        public void DrawOpenList(IEnumerable<PathNode> openList)
        {
            foreach (PathNode node in openList)
            {
                GameObject nodeGO;
                var nodePos = node.GetGridPos();
                if (!nodeGOMap.ContainsKey(nodePos))
                {
                    nodeGO = Instantiate(
                        debugNodePrefab,
                        aStarTilemap.ToCellCenter(nodePos),
                        Quaternion.identity
                    );
                }
                else
                {
                    nodeGO = nodeGOMap[nodePos];
                }
                UpdateNode(nodeGO, node, nodePos, PathfindDebugNodeType.OpenNode);
            }
        }

        public void DrawClosedSet(ISet<Vector3Int> closedSet)
        {
            foreach (Vector3Int nodePos in closedSet)
            {
                GameObject nodeGO;
                if (!nodeGOMap.ContainsKey(nodePos))
                {
                    nodeGO = Instantiate(
                        debugNodePrefab,
                        aStarTilemap.ToCellCenter(nodePos),
                        Quaternion.identity
                    );
                }
                else
                {
                    nodeGO = nodeGOMap[nodePos];
                }
                UpdateNode(nodeGO, null, nodePos, PathfindDebugNodeType.ClosedNode);
            }
        }

        public void DrawState(PathfinderState state)
        {
            DrawStartNode(state.startNode);
            DrawEndNode(state.endNode);
            DrawOpenList(state.openList);
            DrawClosedSet(state.closedSet);
            DrawCurrentNode(state.currentNode);
        }
        #endregion

        private void UpdateNode(
            GameObject go,
            PathNode node,
            Vector3Int gridPos,
            PathfindDebugNodeType nodeType
        )
        {
            PathfindDebugNode debugNode = go.GetComponent<PathfindDebugNode>();
            debugNode.UpdateState(node, nodeType);
            nodeGOMap.TryAdd(gridPos, go);
        }
    }
}
