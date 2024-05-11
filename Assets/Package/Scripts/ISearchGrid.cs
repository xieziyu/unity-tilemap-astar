using UnityEngine;

namespace ZYTools
{
    public interface ISearchGrid
    {
        public bool CanWalk(Vector3Int currentPos, Vector3Int nextPos);
    }
}
