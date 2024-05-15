# A* Pathfinding for Tilemap in Unity

![demo]

## Get Started

You can install this package via Unity Package Manager:

### Install by git

```
https://github.com/xieziyu/unity-tilemap-astar.git?path=/Assets/Package
```

### Simple Usage

1. Add `AStarTilemap` Component to some GameObject.
2. Assign walkable road tilemap and collider tilemaps to it.
3. Use `AStarTilemap.FindPath(Vector3Int startPos, Vector3Int endPos)` to find path.

### Debugger

The debugger implementing `IPathfindDebugger` helps to show the process of pathfinding algorithm.
I provide a sample one with this package. You can import it via `Package Manager / Samples` tab.
Please refer to the `SampleScene` in this repository for more details.

PS: Remember to enable debug mode in `AStarTilemap` and also assign `AStarTilemap` to the debugger.

<!-- LINKS & IMAGES -->
[demo]: ./Docs/demo.gif
