using UnityEngine;
using UnityEngine.EventSystems;

public class DragToSide : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float dragLimit = 200f;
    [SerializeField] private MiniGameController miniGameController;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // (opzionale) disattiva altri input se necessario
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float draggedX = rectTransform.anchoredPosition.x - startPosition.x;

        if (Mathf.Abs(draggedX) >= dragLimit)
        {
            // Completato il minigioco
            miniGameController.EndMiniGame();
        }
        else
        {
            // Ritorna alla posizione iniziale
            rectTransform.anchoredPosition = startPosition;
        }
    }
}

