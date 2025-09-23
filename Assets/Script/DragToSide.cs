using UnityEngine;
using UnityEngine.EventSystems;

public class DragToSide : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private float dragLimit = 200f;
    [SerializeField] private MiniGameController miniGameController;
    [SerializeField] private ItemCombiner itemCombiner;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    private AudioSource audioSource;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        audioSource = GetComponent<AudioSource>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Non permettere il drag se il minigioco non è attivo
        if (miniGameController == null || !miniGameController.IsPlaying) return;

        if (audioSource != null && !audioSource.isPlaying)
            audioSource.Play();

        miniGameController.NotifyDragStarted(); // Notifica che il drag è iniziato

        // Mostra lo sprite post-minigioco usando il metodo pubblico
        if (itemCombiner != null)
            itemCombiner.ShowPostMiniGameSprite(itemCombiner.CurrentCombination);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (miniGameController == null || !miniGameController.IsPlaying) return;

        // Muovi solo orizzontalmente e limita il movimento
        rectTransform.anchoredPosition += new Vector2(eventData.delta.x, 0);
        rectTransform.anchoredPosition = new Vector2(
            Mathf.Clamp(rectTransform.anchoredPosition.x, startPosition.x - dragLimit, startPosition.x + dragLimit),
            startPosition.y
        );
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (miniGameController == null || !miniGameController.IsPlaying) return;

        if (audioSource != null && audioSource.isPlaying)
            audioSource.Stop();

        float draggedX = rectTransform.anchoredPosition.x - startPosition.x;

        if (Mathf.Abs(draggedX) >= dragLimit)
        {
            miniGameController.StartPointerPhase();
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }

        // Nascondi lo sprite post-minigioco
        if (itemCombiner != null)
            itemCombiner.HidePostMiniGameSprite();
    }
}
