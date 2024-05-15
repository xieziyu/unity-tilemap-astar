using UnityEngine;

namespace ZYTools
{
    public interface ISearchGrid
    {
        public bool CanWalk(Vector3Int currentGrid, Vector3Int nextGrid);
    }
}
