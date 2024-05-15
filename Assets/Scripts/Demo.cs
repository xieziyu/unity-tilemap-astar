using System.Collections.Generic;
using UnityEngine;

namespace ZYTools.Demo
{
    public class Demo : MonoBehaviour
    {
        [SerializeField]
        private AStarTilemap astarTilemap;

        [SerializeField]
        private bool stepMode;

        [SerializeField]
        private float debugStepDelay = 0.1f;

        private Camera mainCamera;
        private bool isStepDebugging = false;
        private float stepDelay = 0f;
        private bool isPaused = false;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isStepDebugging)
                {
                    // 中断
                    isPaused = !isPaused;
                }
                else
                {
                    // 更新路径
                    StartPathfinding();
                }
                return;
            }

            if (isPaused)
            {
                return;
            }

            if (isStepDebugging)
            {
                stepDelay += Time.deltaTime;
                if (stepDelay >= debugStepDelay)
                {
                    TriggerNextStep();
                    stepDelay = 0;
                }
                return;
            }

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
                StartPathfinding();
                return;
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                astarTilemap.ResetPath();
                return;
            }
        }

        private void SetStartPoint()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = astarTilemap.ToCellCenter(mousePos);
            astarTilemap.SetStartPos(astarTilemap.ToGridPos(cellPosition));
            astarTilemap.ResetPath();
        }

        private void SetEndPoint()
        {
            Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = astarTilemap.ToCellCenter(mousePos);
            astarTilemap.SetEndPos(astarTilemap.ToGridPos(cellPosition));
            astarTilemap.ResetPath();
        }

        private void TriggerNextStep()
        {
            if (!isStepDebugging)
            {
                return;
            }
            var finished = astarTilemap.StepNext();
            isStepDebugging = !finished;
        }

        private void StartPathfinding()
        {
            astarTilemap.ResetPath();
            if (stepMode)
            {
                astarTilemap.StepStartFindPath();
                isStepDebugging = true;
                stepDelay = 0;
            }
            else
            {
                astarTilemap.FindPath();
            }
            isPaused = false;
        }
    }
}
