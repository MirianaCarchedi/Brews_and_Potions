using UnityEngine;
using UnityEngine.EventSystems;

public class DragToSide : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float dragLimit = 200f;
    [SerializeField] private MiniGameController miniGameController;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        audioSource = GetComponent<AudioSource>(); // Assicurati che ci sia un AudioSource sullo stesso oggetto
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        float draggedX = rectTransform.anchoredPosition.x - startPosition.x;

        if (Mathf.Abs(draggedX) >= dragLimit)
        {
            miniGameController.EndMiniGame();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }
}
