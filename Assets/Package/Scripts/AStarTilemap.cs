using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ZYTools
{
    public class AStarTilemap : MonoBehaviour, ISearchGrid
    {
        [SerializeField]
        private bool allowDiagonals;

        [SerializeField]
        private Tilemap roadTilemap;

        [SerializeField]
        private Tilemap[] colliderTilemaps;

        private Pathfinder pathfinder;

        private void Awake()
        {
            pathfinder = new(this, allowDiagonals);
        }

        public List<PathNode> FindPath(Vector3Int startPos, Vector3Int endPos)
        {
            return pathfinder.FindPath(startPos, endPos);
        }

        public bool CanWalk(Vector3Int currentPos, Vector3Int nextPos)
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
            return true;
        }

        public Vector3Int ToGridXZY(Vector3 position)
        {
            return roadTilemap.WorldToCell(position);
        }

        public Vector3 ToCellCenter(Vector3 position)
        {
            return roadTilemap.GetCellCenterWorld(ToGridXZY(position));
        }
    }
}