using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ZYTools
{
    public class AStarTilemap : MonoBehaviour, ISearchGrid
    {
        [Header("Tilemap Settings")]
        [SerializeField]
        private Tilemap roadTilemap;

        [SerializeField]
        private Tilemap[] colliderTilemaps;

        [Header("Pathfinder Settings")]
        [SerializeField]
        private bool allowDiagonals;

        [Header("Debug")]
        [SerializeField]
        private bool enableDebugMode;

        private IPathfindDebugger debugger;
        private Pathfinder pathfinder;

        private void Awake()
        {
            pathfinder = new(this, allowDiagonals);
        }

        public void SetDebugger(IPathfindDebugger debugger)
        {
            this.debugger = debugger;
        }

        public void SetStartPos(Vector3Int startPos)
        {
            var node = new PathNode(startPos);
            pathfinder.SetStartNode(node);
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                debugger.DrawStartNode(node);
            }
        }

        public void SetEndPos(Vector3Int endPos)
        {
            var node = new PathNode(endPos);
            pathfinder.SetEndNode(node);
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                debugger.DrawEndNode(node);
            }
        }

        public List<PathNode> FindPath()
        {
            var pathList = pathfinder.FindPath();
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                foreach (var step in pathList)
                {
                    debugger.DrawStepNode(step);
                }
            }
            return pathList;
        }

        public void StepStartFindPath()
        {
            pathfinder.StepStartFindPath();
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                debugger.Clear();
                debugger.DrawState(pathfinder.GetState());
            }
        }

        public void ResetPath()
        {
            pathfinder.ResetPaths();
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                debugger.Clear();
                debugger.DrawStartNode(pathfinder.GetStartNode());
                debugger.DrawEndNode(pathfinder.GetEndNode());
            }
        }

        public bool StepNext()
        {
            bool finished = pathfinder.StepNext();
            // Debugger
            if (enableDebugMode && debugger != null)
            {
                debugger.DrawState(pathfinder.GetState());

                if (finished)
                {
                    var pathList = pathfinder.GetFinalPath();
                    foreach (var step in pathList)
                    {
                        debugger.DrawStepNode(step);
                    }
                    debugger.DrawStartNode(pathfinder.GetStartNode());
                    debugger.DrawEndNode(pathfinder.GetEndNode());
                }
            }
            return finished;
        }

        public List<PathNode> GetFinalPath()
        {
            return pathfinder.GetFinalPath();
        }

        public List<PathNode> FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            SetStartPos(startPos);
            SetEndPos(endPos);
            return FindPath();
        }

        #region Interface Implementation
        public bool HasLink(Vector3Int currentPos, Vector3Int nextPos)
        {
            if (!roadTilemap.HasTile(nextPos))
            {
                return false;
            }

            foreach (var tilemap in colliderTilemaps)
            {
                if (tilemap.HasTile(nextPos))
                {
                    return false;
                }
            }

            // Fix Diagonal Issue
            if (allowDiagonals)
            {
                if (currentPos.x != nextPos.x && currentPos.y != nextPos.y)
                {
                    foreach (var tilemap in colliderTilemaps)
                    {
                        if (
                            tilemap.HasTile(new Vector3Int(currentPos.x, nextPos.y, 0))
                            || tilemap.HasTile(new Vector3Int(nextPos.x, currentPos.y, 0))
                        )
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        #endregion

        #region Helper Methods
        public Vector3Int ToGridPos(Vector3 position)
        {
            return roadTilemap.WorldToCell(position);
        }

        public Vector3 ToCellCenter(Vector3 position)
        {
            return roadTilemap.GetCellCenterWorld(ToGridPos(position));
        }
        #endregion
    }
}
