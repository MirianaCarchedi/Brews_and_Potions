using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public class MiniGameController : MonoBehaviour
{
   
    [SerializeField] private TutorialManager tutorialManager; // <-- aggiungi questa riga

    [Header("Modalità DRAG")]
    public RectTransform draggableImage;
    public float successThreshold = 0f; // in drag mode basta muoverlo
    public float minX = -200f;
    public float maxX = 200f;

    [Header("Modalità POINTER")]
    public RectTransform pointer;
    public Transform pointA;
    public Transform pointB;
    public RectTransform safeZoneA;  // Safezone per resultN
    public RectTransform safeZoneB;  // Safezone per resultN
    public RectTransform safeZoneC;  // Safezone per resultP
    public float moveSpeed = 200f;
    [SerializeField] private ItemCombiner itemCombiner;

    [Header("UI Panel")]
    public GameObject pointerPanel;
    public GameObject panelButtonTutorial;

    [Header("Popup risultati")]
    public List<GameObject> resultPopups; // popup per resultN e resultP
    private HashSet<string> seenTags = new HashSet<string>();

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

    private bool dragStarted = false;

    public void SetCurrentResultTag(string tag)
    {
        currentResultTag = tag;
    }

    public void StartMiniGame(string resultTag, Action<bool, string> onResult)
    {
        currentResultTag = resultTag;
        onComplete = onResult;
        isPlaying = true;
        dragStarted = false;
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

        if (success && !string.IsNullOrEmpty(resultTag) && !seenTags.Contains(resultTag))
        {
            seenTags.Add(resultTag);
            foreach (var popup in resultPopups)
            {
                if (popup != null && popup.name == resultTag)
                {
                    popup.SetActive(true);
                    StartCoroutine(DestroyPopupAfterDelay(popup, 5f)); //  Distrugge completamente dopo 5 secondi
                    break;
                }
            }
        }

        // Istanzia prefab di fallimento se necessario
        if (!success && failPrefab != null)
        {
            Instantiate(failPrefab, pointerPanel != null ? pointerPanel.transform : transform);
        }

        onComplete?.Invoke(success, resultTag);
        onComplete = null;
        currentResultTag = "";
    }

    private IEnumerator DestroyPopupAfterDelay(GameObject popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popup != null)
            Destroy(popup);
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
        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (!dragStarted || dragScript == null || !dragScript.enabled)
            return;

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

        if (dragStarted && Input.GetMouseButtonUp(0))
        {
            // Appena rilasciato, passo al passo successivo
            StartPointerPhase();
        }
    }

    public void NotifyDragStarted()
    {
        dragStarted = true;
    }

    public void StartPointerPhase()
    {
        if (tutorialManager != null)
        {
            tutorialManager.GoToStep(10);
        }

        // Mostra lo sprite post-minigioco usando il metodo pubblico
        if (itemCombiner != null)
            itemCombiner.ShowPostMiniGameSprite(itemCombiner.CurrentCombination);
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
            // Nascondi lo sprite post-minigioco
            if (itemCombiner != null)
                itemCombiner.HidePostMiniGameSprite();
            pointerStopped = true;

            if (RectTransformUtility.RectangleContainsScreenPoint(safeZoneA, pointer.position, null) ||
                RectTransformUtility.RectangleContainsScreenPoint(safeZoneB, pointer.position, null))
            {
                EndMiniGame(true, "resultN");
                // Pozione buona
                if (tutorialManager != null)
                    tutorialManager.GoToStep(12); 
            }
            else if (RectTransformUtility.RectangleContainsScreenPoint(safeZoneC, pointer.position, null))
            {
                EndMiniGame(true, "resultP");
                // Pozione perfetta
                if (tutorialManager != null)
                    tutorialManager.GoToStep(11); 
            }
            else
            {
                EndMiniGame(false, currentResultTag);
                // Pozione fallita
                if (tutorialManager != null)
                    tutorialManager.GoToStep(13); 
            }

        }
    }
}
