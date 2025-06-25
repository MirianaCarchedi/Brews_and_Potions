using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
                //  Salva prefab per trasferimento alla scena successiva
                CombinationTransfer.resultPrefabToTransfer = combo.resultPrefab;

                return;
            }
        }

        Debug.Log("Nessuna combinazione valida trovata per: " + tagA + " + " + tagB);
    }
}
