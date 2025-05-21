using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private void Start()
    {
        LoadItem();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;
        if (droppedObject == null) return;

        DragDrop draggedDrop = droppedObject.GetComponent<DragDrop>();
        if (draggedDrop == null) return;

        Transform currentSlot = transform;
        Transform otherSlot = draggedDrop.OriginalParent;

        // If this slot is occupied, swap
        if (currentSlot.childCount > 0)
        {
            GameObject currentChild = currentSlot.GetChild(0).gameObject;
            DragDrop currentDragDrop = currentChild.GetComponent<DragDrop>();

            currentChild.transform.SetParent(otherSlot, false);
            currentDragDrop.ResetRectTransform();
            SavePosition(currentChild.name, otherSlot.name);
        }

        droppedObject.transform.SetParent(currentSlot, false);
        draggedDrop.ResetRectTransform();
        SavePosition(droppedObject.name, currentSlot.name);
    }

    private void SavePosition(string itemName, string slotName)
    {
        PlayerPrefs.SetString("Item_" + itemName, slotName);
        PlayerPrefs.Save();
    }

    private void LoadItem()
    {
        foreach (Transform child in transform.root.GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue;

            string savedSlot = PlayerPrefs.GetString("Item_" + child.name, "");
            if (savedSlot == name)
            {
                child.SetParent(transform, false);
                var dd = child.GetComponent<DragDrop>();
                if (dd != null) dd.ResetRectTransform();
            }
        }
    }
}
