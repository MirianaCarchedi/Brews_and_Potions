using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;
    public bool droppedInSlot = false;
    private Vector2 _originalPosition;
    public Vector2 OriginalPosition
    {
        get { return _originalPosition; }
        set { _originalPosition = value; }
    }




    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // Carica la posizione salvata, se esiste
        if (PlayerPrefs.HasKey(gameObject.name + "_x") && PlayerPrefs.HasKey(gameObject.name + "_y"))
        {
            float x = PlayerPrefs.GetFloat(gameObject.name + "_x");
            float y = PlayerPrefs.GetFloat(gameObject.name + "_y");
            rectTransform.anchoredPosition = new Vector2(x, y);
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        originalPosition = rectTransform.anchoredPosition; // salva posizione originale
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        droppedInSlot = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (!droppedInSlot)
        {
            rectTransform.anchoredPosition = originalPosition;
        }

        SavePosition(); // salva la posizione anche se spostato
    }

    public void SavePosition()
    {
        PlayerPrefs.SetFloat(gameObject.name + "_x", rectTransform.anchoredPosition.x);
        PlayerPrefs.SetFloat(gameObject.name + "_y", rectTransform.anchoredPosition.y);
        PlayerPrefs.Save();
    }

}
