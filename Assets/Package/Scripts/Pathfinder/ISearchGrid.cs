using UnityEngine;

namespace ZYTools
{
    public interface ISearchGrid
    {
        public bool HasLink(Vector3Int currentGrid, Vector3Int nextGrid);
    }
}
