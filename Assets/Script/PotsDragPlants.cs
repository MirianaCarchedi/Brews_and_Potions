using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PotsDragPlants : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject prefab;
    public Canvas canvas;
    public Transform defaultParent;

    private GameObject instance;

    // 🔹 Dizionario statico per tenere traccia degli oggetti attivi per prefab
    private static Dictionary<GameObject, GameObject> activePrefabs = new Dictionary<GameObject, GameObject>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (prefab == null || canvas == null || defaultParent == null) return;

        // ❌ Blocca spawn e drag se esiste già un'istanza di questo prefab
        if (activePrefabs.ContainsKey(prefab) && activePrefabs[prefab] != null)
        {
            // Non fare nulla, blocca totalmente
            instance = null;
            return;
        }

        // 🔹 Spawn del prefab nel parent di default
        instance = Instantiate(prefab, defaultParent);

        // Salva nell'elenco degli attivi
        activePrefabs[prefab] = instance;

        // Imposta OriginalParent per DragDrop
        var drag = instance.GetComponent<DragDrop>();
        if (drag != null)
        {
            drag.OriginalParent = defaultParent;
        }

        // Posiziona sotto il cursore
        RectTransform rt = instance.GetComponent<RectTransform>();
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            defaultParent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );
        rt.localPosition = localPoint;

        // 🔹 Avvia il drag solo sul nuovo oggetto appena spawnato
        eventData.pointerDrag = instance;
        ExecuteEvents.Execute(instance, eventData, ExecuteEvents.beginDragHandler);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (instance != null)
        {
            ExecuteEvents.Execute(instance, eventData, ExecuteEvents.dragHandler);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (instance != null)
        {
            ExecuteEvents.Execute(instance, eventData, ExecuteEvents.endDragHandler);
            instance = null;
        }
    }

    // 🔹 Metodo statico per distruggere un prefab e liberare lo spawn
    public static void ClearInstance(GameObject prefabToClear)
    {
        if (activePrefabs.ContainsKey(prefabToClear) && activePrefabs[prefabToClear] != null)
        {
            GameObject.Destroy(activePrefabs[prefabToClear]);
            activePrefabs[prefabToClear] = null;
        }
    }
}

