using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public DragDrop currentItem;

    public void OnDrop(PointerEventData eventData)
    {
        var draggedItem = eventData.pointerDrag?.GetComponent<DragDrop>();
        if (draggedItem == null) return;

        // Ignora se stesso
        if (draggedItem == currentItem) return;

        var sourceSlot = draggedItem.ParentSlot;
        var sourcePos = draggedItem.OriginalPosition;

        // Se questo slot ha già un oggetto...
        if (currentItem != null)
        {
            var otherItem = currentItem;
            var otherRect = otherItem.GetComponent<RectTransform>();

            // Sposta oggetto già nello slot nella posizione originaria di quello trascinato
            otherRect.anchoredPosition = sourcePos;
            otherItem.OriginalPosition = sourcePos;
            otherItem.droppedInSlot = true;

            if (sourceSlot != null)
            {
                sourceSlot.currentItem = otherItem;
                otherItem.ParentSlot = sourceSlot;
            }
        }
        else if (sourceSlot != null)
        {
            sourceSlot.currentItem = null;
        }

        // Posiziona il nuovo oggetto in questo slot
        RectTransform draggedRect = draggedItem.GetComponent<RectTransform>();
        draggedRect.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        draggedItem.OriginalPosition = draggedRect.anchoredPosition;
        draggedItem.droppedInSlot = true;
        draggedItem.ParentSlot = this;
        currentItem = draggedItem;
    }
}
