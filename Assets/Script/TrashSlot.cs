using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TrashSlot : MonoBehaviour, IDropHandler
{
    [Tooltip("Tag che non devono essere distrutti")]
    public List<string> allowedTags = new List<string> { "Artemisia", "Belladonna", "Verbena", "Mandrake" };

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        string tag = dropped.tag;
        if (!allowedTags.Contains(tag))
        {
            Destroy(dropped);
        }
        else
        {
            // Se vuoi rimetterlo al suo posto originale
            dropped.transform.SetParent(dropped.GetComponent<DragDrop>().OriginalParent);
            dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}
