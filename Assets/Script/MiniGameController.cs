using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MiniGameController : MonoBehaviour
{
    [Header("Modalità DRAG")]
    public RectTransform draggableImage;
    public float successThreshold = 200f;
    public float minX = -200f;
    public float maxX = 200f;

    [Header("Modalità POINTER")]
    public RectTransform pointer;
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZoneA;  // Safezone per primo risultato
    public RectTransform safeZoneB;  // Safezone per primo risultato
    public RectTransform safeZoneC;  // Safezone per secondo risultato
    public float moveSpeed = 200f;

    [Header("UI Panel")]
    public GameObject pointerPanel;

    [Header("Prefab fallimento")]
    public GameObject failPrefab;

    private Vector3 startPosition;
    private Action<bool, string> onComplete;
    public bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    private enum GameMode { Drag, Pointer }
    private GameMode currentMode = GameMode.Drag;

    private Vector3 pointerTarget;
    private bool pointerStopped = false;

    private string currentResultTag = "";

    public void SetCurrentResultTag(string tag)
    {
        currentResultTag = tag;
    }

    public void StartMiniGame(string resultTag, Action<bool, string> onResult)
    {
        currentResultTag = resultTag;
        onComplete = onResult;
        isPlaying = true;
        gameObject.SetActive(true);

        currentMode = GameMode.Drag;
        startPosition = draggableImage.localPosition;

        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (dragScript != null)
            dragScript.enabled = true;

        if (pointerPanel != null)
            pointerPanel.SetActive(false);
    }

    public void EndMiniGame(bool success, string resultTag)
    {
        if (!isPlaying) return;

        isPlaying = false;
        gameObject.SetActive(false);

        if (pointerPanel != null)
            pointerPanel.SetActive(false);

        draggableImage.localPosition = startPosition;

        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (dragScript != null)
            dragScript.enabled = false;

        // Se fallimento e prefab fallimento assegnato, lo istanzia
        if (!success && failPrefab != null)
        {
            Instantiate(failPrefab, pointerPanel != null ? pointerPanel.transform : transform);
        }

        onComplete?.Invoke(success, resultTag);
        onComplete = null;
        currentResultTag = "";
    }

    private void Update()
    {
        if (!isPlaying) return;

        if (currentMode == GameMode.Drag)
            HandleDragMode();
        else if (currentMode == GameMode.Pointer)
            HandlePointerMode();
    }

    private void HandleDragMode()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                draggableImage.parent as RectTransform,
                Input.mousePosition,
                null,
                out mousePos
            );

            Vector3 newPos = draggableImage.localPosition;
            newPos.x = Mathf.Clamp(mousePos.x, minX, maxX);
            newPos.y = startPosition.y;
            draggableImage.localPosition = newPos;
        }

        float distanceMoved = Mathf.Abs(draggableImage.localPosition.x - startPosition.x);
        if (distanceMoved >= successThreshold)
        {
            StartPointerPhase();
        }
    }

    public void StartPointerPhase()
    {
        currentMode = GameMode.Pointer;
        pointerStopped = false;
        pointer.position = pointA.position;
        pointerTarget = pointB.position;

        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (dragScript != null)
            dragScript.enabled = false;

        if (pointerPanel != null)
            pointerPanel.SetActive(true);
    }

    private void HandlePointerMode()
    {
        if (pointerStopped) return;

        pointer.position = Vector3.MoveTowards(pointer.position, pointerTarget, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(pointer.position, pointA.position) < 0.1f)
            pointerTarget = pointB.position;
        else if (Vector3.Distance(pointer.position, pointB.position) < 0.1f)
            pointerTarget = pointA.position;

        if (Input.GetMouseButtonDown(0))
        {
            pointerStopped = true;

            // Controllo delle safezone
            if (RectTransformUtility.RectangleContainsScreenPoint(safeZoneA, pointer.position, null) ||
                RectTransformUtility.RectangleContainsScreenPoint(safeZoneB, pointer.position, null))
            {
                EndMiniGame(true, "resultN");
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(safeZoneC, pointer.position, null))
            {
                EndMiniGame(true, "resultP");
            }
            else
            {
                EndMiniGame(false, currentResultTag);
            }
        }
    }
}

