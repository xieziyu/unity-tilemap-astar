using System.Collections.Generic;
using UnityEngine;

namespace ZYTools
{
    public class AStarTilemapPathfindDebugger : MonoBehaviour, IPathfindDebugger
    {
        [SerializeField]
        private GameObject debugNodePrefab;

        private AStarTilemap aStarTilemap;
        private Dictionary<Vector3Int, GameObject> nodeGOMap = new();
        private GameObject startNodeGO;
        private GameObject endNodeGO;

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

        public void Init()
        {
            foreach (var go in nodeGOMap.Values)
            {
                Destroy(go);
            }
            nodeGOMap.Clear();
        }

        public void SetTilemap(AStarTilemap aStarTilemap)
        {
            this.aStarTilemap = aStarTilemap;
        }

        public void DrawState(
            PathNode currentNode,
            PathNode startNode,
            PathNode endNode,
            List<PathNode> openList,
            HashSet<Vector3Int> closedSet
        )
        {
            // Start Node
            if (startNodeGO == null)
            {
                startNodeGO = Instantiate(
                    debugNodePrefab,
                    aStarTilemap.ToCellCenter(startNode.GetGridPos()),
                    Quaternion.identity
                );
                UpdateNode(
                    startNodeGO,
                    startNode,
                    startNode.GetGridPos(),
                    PathfindDebugNodeType.StartNode
                );
            }

            // End Node
            if (endNodeGO == null)
            {
                endNodeGO = Instantiate(
                    debugNodePrefab,
                    aStarTilemap.ToCellCenter(endNode.GetGridPos()),
                    Quaternion.identity
                );
                UpdateNode(endNodeGO, endNode, endNode.GetGridPos(), PathfindDebugNodeType.EndNode);
            }

            // Open List
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

            // Closed Set
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

            // Current Node
            if (currentNode != null)
            {
                if (!nodeGOMap.TryGetValue(currentNode.GetGridPos(), out GameObject currentGO))
                {
                    currentGO = Instantiate(
                        debugNodePrefab,
                        aStarTilemap.ToCellCenter(currentNode.GetGridPos()),
                        Quaternion.identity
                    );
                }
                UpdateNode(
                    currentGO,
                    currentNode,
                    currentNode.GetGridPos(),
                    PathfindDebugNodeType.CurrentNode
                );
            }
        }

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
