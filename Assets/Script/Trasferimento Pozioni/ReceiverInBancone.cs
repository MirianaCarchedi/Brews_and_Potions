using UnityEngine;

public class ReceiverInBancone : MonoBehaviour
{
    public Transform targetSlot; 

    void Start()
    {
        if (CombinationTransfer.resultPrefabToTransfer != null)
        {
            GameObject newItem = GameObject.Instantiate(CombinationTransfer.resultPrefabToTransfer);
            newItem.transform.SetParent(targetSlot, false);

            RectTransform rt = newItem.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            CombinationTransfer.resultPrefabToTransfer = null; // reset
        }
    }
}

