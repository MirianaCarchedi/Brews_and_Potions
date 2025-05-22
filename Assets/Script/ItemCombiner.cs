using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemCombiner : MonoBehaviour
{

    [Header("Slot di input")]
    public Transform slotA;
    public Transform slotB;

    [Header("Slot di output (risultato)")]
    public Transform resultSlot;

    [Header("Bottone di combinazione")]
    public Button combineButton;

    [System.Serializable]
    public class CombinationData
    {
        public string tag1; 
        public string tag2; 

        public GameObject resultPrefab;
    }

    [Header("Combinazioni possibili")]
    public List<CombinationData> combinations = new List<CombinationData>();


    private void Update()
    {
        bool readyToCombine =
            slotA.childCount > 0 &&
            slotB.childCount > 0 &&
            resultSlot.childCount == 0;

        combineButton.interactable = readyToCombine;
    }

    public void Combine()
    {
        if (slotA.childCount == 0 || slotB.childCount == 0 || resultSlot.childCount > 0)
            return;

        GameObject objA = slotA.GetChild(0).gameObject;
        GameObject objB = slotB.GetChild(0).gameObject;

        string tagA = objA.tag;
        string tagB = objB.tag;

        foreach (var combo in combinations)
        {
            bool match =
                (combo.tag1 == tagA && combo.tag2 == tagB) ||
                (combo.tag1 == tagB && combo.tag2 == tagA);

            if (match)
            {
                // Rimuove oggetti originali
                Destroy(objA);
                Destroy(objB);

                // ✅ Crea solo UNA volta il nuovo oggetto combinato
                GameObject newItem = Instantiate(combo.resultPrefab);
                newItem.transform.SetParent(resultSlot, false);

                RectTransform rt = newItem.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                rt.localScale = Vector3.one;

                return;
            }
        }

        Debug.Log("Nessuna combinazione valida trovata per: " + tagA + " + " + tagB);
    }

}


