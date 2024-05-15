using System.Collections.Generic;
using UnityEngine;

namespace ZYTools.Demo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField]
        private AStarTilemap astarTilemap;

        [SerializeField]
        private bool debugMode;

        [SerializeField]
        private GameObject startPointPrefab;

        [SerializeField]
        private GameObject endPointPrefab;

        [SerializeField]
        private GameObject stepPrefab;

        private Camera mainCamera;
        private GameObject startPoint;
        private GameObject endPoint;
        private List<GameObject> stepList = new();
        private bool isDebugging = false;
        private readonly float debugStepDelay = 0.1f;
        private float debugDelay = 0f;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetStartPoint();
                return;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                SetEndPoint();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdatePath();
                return;
            }

            if (isDebugging)
            {
                debugDelay += Time.deltaTime;
                if (debugDelay >= debugStepDelay)
                {
                    TriggerDebugStep();
                    debugDelay = 0;
                }
            }
        }

        private void SetStartPoint()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = astarTilemap.ToCellCenter(mousePos);
            if (startPoint == null)
            {
                startPoint = Instantiate(startPointPrefab, cellPosition, Quaternion.identity);
            }
            else
            {
                startPoint.transform.position = cellPosition;
            }
            ClearPath();
        }

        private void SetEndPoint()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = astarTilemap.ToCellCenter(mousePos);
            if (endPoint == null)
            {
                endPoint = Instantiate(endPointPrefab, cellPosition, Quaternion.identity);
            }
            else
            {
                endPoint.transform.position = cellPosition;
            }
            ClearPath();
        }

        private void TriggerDebugStep()
        {
            if (!isDebugging)
            {
                return;
            }
            var finished = astarTilemap.DebugNextStep();
            isDebugging = !finished;
            if (finished)
            {
                var paths = astarTilemap.GetFinalPath();
                DrawPath(paths);
            }
        }

        private void UpdatePath()
        {
            if (startPoint == null || endPoint == null)
            {
                return;
            }
            ClearPath();
            var starPos = astarTilemap.ToGridXYZ(startPoint.transform.position);
            var endPos = astarTilemap.ToGridXYZ(endPoint.transform.position);

            // Trigger Debug Mode:
            if (debugMode)
            {
                astarTilemap.DebugFindPath(starPos, endPos);
                isDebugging = true;
                debugDelay = 0;
                return;
            }

            var paths = astarTilemap.FindPath(starPos, endPos);
            DrawPath(paths);
        }

        private void DrawPath(List<PathNode> paths)
        {
            foreach (var path in paths)
            {
                var step = Instantiate(
                    stepPrefab,
                    astarTilemap.ToCellCenter(path.GetGridPos()),
                    Quaternion.identity
                );
                stepList.Add(step);
            }
        }

        private void ClearPath()
        {
            foreach (var step in stepList)
            {
                Destroy(step);
            }
            stepList.Clear();
        }
    }
}
