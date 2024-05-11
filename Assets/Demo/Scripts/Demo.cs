using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZYTools;

public class Demo : MonoBehaviour
{
    [SerializeField]
    private AStarTilemap astarTilemap;

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

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetStartPoint();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            SetEndPoint();
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
        UpdatePath();
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
        UpdatePath();
    }

    private void UpdatePath()
    {
        if (startPoint == null || endPoint == null)
        {
            return;
        }
        ClearPath();
        var starPos = astarTilemap.ToGridXZY(startPoint.transform.position);
        var endPos = astarTilemap.ToGridXZY(endPoint.transform.position);
        var paths = astarTilemap.FindPath(starPos, endPos);
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