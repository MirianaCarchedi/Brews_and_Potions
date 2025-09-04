using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotsDragPlants : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Prefab da spawnare (deve avere DragDrop)")]
    public GameObject prefab;

    [Header("Canvas di riferimento")]
    public Canvas canvas;

    // L'istanza attualmente trascinata
    private GameObject instance;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab == null || canvas == null) return;

        // Crea l'istanza come figlia del canvas
        instance = Instantiate(prefab, canvas.transform);

        // Posiziona subito sotto il cursore
        RectTransform rt = instance.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );
        rt.localPosition = localPoint;

        // Inoltra "inizio drag" al prefab
        ExecuteEvents.Execute(instance, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (instance != null)
        {
            // Inoltra "drag" al prefab
            ExecuteEvents.Execute(instance, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (instance != null)
        {
            // Inoltra "fine drag" al prefab
            ExecuteEvents.Execute(instance, eventData, ExecuteEvents.endDragHandler);
            instance = null;
        }
    }
}
