using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public DragDrop currentItem; // L'oggetto attualmente nel slot (può essere null)

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            DragDrop draggedItem = eventData.pointerDrag.GetComponent<DragDrop>();
            if (draggedItem == null) return;

            RectTransform draggedRect = draggedItem.GetComponent<RectTransform>();

            // Se c'è già un oggetto nello slot, scambiamo le posizioni
            if (currentItem != null && currentItem != draggedItem)
            {
                Vector2 tempPos = currentItem.GetComponent<RectTransform>().anchoredPosition;
                currentItem.GetComponent<RectTransform>().anchoredPosition = draggedItem.OriginalPosition;
                currentItem.OriginalPosition = draggedItem.OriginalPosition;

                // Aggiorna il riferimento del vecchio oggetto al suo nuovo slot (se vuoi)
            }

            // Sposta il nuovo oggetto in questo slot
            draggedRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;

            // Aggiorna gli stati
            draggedItem.droppedInSlot = true;
            draggedItem.OriginalPosition = draggedRect.anchoredPosition;

            currentItem = draggedItem;
        }
    }
}


