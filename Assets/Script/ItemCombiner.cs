using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
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
    public class ForbiddenCombination
    {
        public string tag1;
        public string tag2;
    }

    [Header("Combinazioni proibite (skippano minigioco e danno fail)")]
    public List<ForbiddenCombination> forbiddenCombinations = new List<ForbiddenCombination>();

    [System.Serializable]
    public class CombinationData
    {
        public string tag1;
        public string tag2;
        public GameObject resultPrefabN;
        public GameObject resultPrefabP;
        public Sprite previewSprite;
        public Sprite postMiniGameSprite;
        public string resultTag;

        [Header("Popup specifici per risultato")]
        public GameObject resultPopupN;
        public GameObject resultPopupP;
    }

    [Header("Combinazioni possibili")]
    public List<CombinationData> combinations = new List<CombinationData>();

    private CombinationData currentCombination = null;

    public CombinationData CurrentCombination
    {
        get { return currentCombination; }
    }

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

    public void ShowPostMiniGameSprite(CombinationData combo)
    {
        if (resultPreviewImage != null && combo.postMiniGameSprite != null)
        {
            resultPreviewImage.sprite = combo.postMiniGameSprite;
            SetImageAlpha(resultPreviewImage, 1f);
        }
    }

    public void HidePostMiniGameSprite()
    {
        if (resultPreviewImage != null)
        {
            SetImageAlpha(resultPreviewImage, 0f);
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

        // ✅ Controllo combinazioni proibite
        foreach (var forbidden in forbiddenCombinations)
        {
            bool match = (forbidden.tag1 == tagA && forbidden.tag2 == tagB) ||
                         (forbidden.tag1 == tagB && forbidden.tag2 == tagA);

            if (match)
            {
                if (failResultPrefab != null)
                {
                    GameObject failItem = Instantiate(failResultPrefab);
                    failItem.transform.SetParent(resultSlot, false);
                    RectTransform rt = failItem.GetComponent<RectTransform>();
                    rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = Vector3.one;
                }

                Debug.Log("Combinazione proibita: " + tagA + " + " + tagB);
                return; // esce subito senza minigioco
            }
        }

        // ✅ Combinazioni normali
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
                        GameObject popupToShow = null;

                        if (resultTag == "resultN" && combo.resultPrefabN != null)
                        {
                            newItem = Instantiate(combo.resultPrefabN);
                            popupToShow = combo.resultPopupN;
                        }
                        else if (resultTag == "resultP" && combo.resultPrefabP != null)
                        {
                            newItem = Instantiate(combo.resultPrefabP);
                            popupToShow = combo.resultPopupP;
                        }

                        if (newItem != null)
                        {
                            newItem.transform.SetParent(resultSlot, false);
                            RectTransform rt = newItem.GetComponent<RectTransform>();
                            rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
                            rt.anchoredPosition = Vector2.zero;
                            rt.localScale = Vector3.one;
                        }

                        if (popupToShow != null)
                        {
                            popupToShow.SetActive(true);
                            StartCoroutine(DeactivatePopupAfterDelay(popupToShow, 2.5f));
                        }
                    }
                    else
                    {
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

                    if (success && resultPreviewImage != null && combo.postMiniGameSprite != null)
                    {
                        resultPreviewImage.sprite = combo.postMiniGameSprite;
                        SetImageAlpha(resultPreviewImage, 0f);
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

    private IEnumerator DeactivatePopupAfterDelay(GameObject popup, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (popup != null)
            popup.SetActive(false);
    }
}
