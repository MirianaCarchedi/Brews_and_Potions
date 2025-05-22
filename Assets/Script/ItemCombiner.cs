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

                // Crea il risultato nello slot dedicato
                GameObject result = Instantiate(combo.resultPrefab, resultSlot);
                result.name = combo.resultPrefab.name;

                // Optional: resetta trasform
                RectTransform rt = result.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.localScale = Vector3.one;
                    rt.anchoredPosition = Vector2.zero;
                    rt.offsetMin = Vector2.zero;
                    rt.offsetMax = Vector2.zero;
                }

                return;
            }
        }

        Debug.Log("Nessuna combinazione valida trovata per: " + tagA + " + " + tagB);
    }
}


