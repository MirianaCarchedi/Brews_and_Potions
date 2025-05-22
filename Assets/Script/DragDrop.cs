using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public Transform OriginalParent { get; set; }

    [SerializeField] private Canvas canvas;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
         if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
            canvas = FindObjectOfType<Canvas>(); 
    
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OriginalParent = transform.parent;
        transform.SetParent(canvas.transform); // Move to top to prevent being hidden
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // If not dropped on a slot, return to original slot
        if (transform.parent == canvas.transform)
        {
            transform.SetParent(OriginalParent, false);
            ResetRectTransform();
        }
    }

    public void ResetRectTransform()
    {
        RectTransform rt = GetComponent<RectTransform>();

        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 100);
        rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 100);

        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.localScale = Vector3.one;
        rt.localPosition = Vector3.zero;
    }

}
