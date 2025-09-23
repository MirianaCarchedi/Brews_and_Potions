using UnityEngine;
using UnityEngine.UI;
using System;
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

    public MiniGameController miniGameController; // da assegnare nell'Inspector

    [Header("Immagine preview")]
    public Image resultPreviewImage;

    public GameObject BrewButton;

    [Header("Prefab risultato fallimento")]
    public GameObject failResultPrefab;

    [System.Serializable]
    public class CombinationData
    {
        public string tag1;
        public string tag2;
        public GameObject resultPrefabN;  // prefab per safeZone A/B
        public GameObject resultPrefabP;  // prefab per safeZone C
        public Sprite previewSprite;
        public Sprite postMiniGameSprite;
        public string resultTag;          // tag univoco della combinazione
    }

    [Header("Combinazioni possibili")]
    public List<CombinationData> combinations = new List<CombinationData>();

    private CombinationData currentCombination = null;

    private void Update()
    {
        bool readyToCombine =
            slotA.childCount > 0 &&
            slotB.childCount > 0 &&
            resultSlot.childCount == 0;

        BrewButton.SetActive(readyToCombine);
        combineButton.interactable = readyToCombine;

        if (currentCombination != null && resultSlot.childCount == 0 && !miniGameController.IsPlaying && resultPreviewImage != null)
        {
            SetImageAlpha(resultPreviewImage, 0f);
            currentCombination = null;
        }
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
            bool match = (combo.tag1 == tagA && combo.tag2 == tagB) ||
                         (combo.tag1 == tagB && combo.tag2 == tagA);

            if (match)
            {
                currentCombination = combo;

                if (resultPreviewImage != null && combo.previewSprite != null)
                {
                    resultPreviewImage.sprite = combo.previewSprite;
                    SetImageAlpha(resultPreviewImage, 1f);
                }

                miniGameController.SetCurrentResultTag(combo.resultTag);
                miniGameController.StartMiniGame(combo.resultTag, (success, resultTag) =>
                {
                    if (success)
                    {
                        GameObject newItem = null;

                        // Scegli il prefab corretto in base al tag restituito dal minigioco
                        if (resultTag == "resultN" && combo.resultPrefabN != null)
                            newItem = Instantiate(combo.resultPrefabN);
                        else if (resultTag == "resultP" && combo.resultPrefabP != null)
                            newItem = Instantiate(combo.resultPrefabP);

                        if (newItem != null)
                        {
                            newItem.transform.SetParent(resultSlot, false);
                            RectTransform rt = newItem.GetComponent<RectTransform>();
                            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                            rt.anchoredPosition = Vector2.zero;
                            rt.localScale = Vector3.one;
                        }
                    }
                    else
                    {
                        // Prefab unico per fallimento
                        if (failResultPrefab != null)
                        {
                            GameObject failItem = Instantiate(failResultPrefab);
                            failItem.transform.SetParent(resultSlot, false);
                            RectTransform rt = failItem.GetComponent<RectTransform>();
                            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                            rt.anchoredPosition = Vector2.zero;
                            rt.localScale = Vector3.one;
                        }
                    }

                    // Aggiorna sprite post minigioco
                    if (success && resultPreviewImage != null && combo.postMiniGameSprite != null)
                    {
                        resultPreviewImage.sprite = combo.postMiniGameSprite;
                        SetImageAlpha(resultPreviewImage, 1f);
                    }
                });

                return;
            }
        }

        Debug.Log("Nessuna combinazione valida trovata per: " + tagA + " + " + tagB);
    }

    public void DestroyPlants()
    {
        if (slotA.childCount > 0)
            Destroy(slotA.GetChild(0).gameObject);

        if (slotB.childCount > 0)
            Destroy(slotB.GetChild(0).gameObject);
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}
