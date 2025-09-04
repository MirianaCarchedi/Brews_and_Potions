using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

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

    public Transform sourceSlot;
    public Transform targetSlot;


    [System.Serializable]
    public class CombinationData
    {
        public string tag1;
        public string tag2;

        public GameObject resultPrefab;

        public Sprite previewSprite;         // sprite prima del minigioco
        public Sprite postMiniGameSprite;    // sprite dopo il minigioco
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
        if (readyToCombine)
        {
            BrewButton.SetActive(true);
        }
        combineButton.interactable = readyToCombine;

        // Se il risultato è stato rimosso dallo slot e non siamo in minigioco
        if (currentCombination != null && resultSlot.childCount == 0 && !miniGameController.IsPlaying && resultPreviewImage != null)
        {
            // Imposta alpha a 0 perché slot vuoto e minigioco finito
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
                    SetImageAlpha(resultPreviewImage, 1f);  // Rendi visibile
                }

                miniGameController.StartMiniGame(() =>
                {
                    BrewButton.SetActive(false);
                    if (resultPreviewImage != null && combo.postMiniGameSprite != null)
                    {
                        resultPreviewImage.sprite = combo.postMiniGameSprite;
                        SetImageAlpha(resultPreviewImage, 1f); // Assicura visibilità anche qui
                    }

                    GameObject newItem = Instantiate(combo.resultPrefab);
                    newItem.transform.SetParent(resultSlot, false);

                    RectTransform rt = newItem.GetComponent<RectTransform>();
                    rt.anchorMin = new Vector2(0.5f, 0.5f);
                    rt.anchorMax = new Vector2(0.5f, 0.5f);
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = Vector3.one;
                });

                return;
            }
        }

        Debug.Log("Nessuna combinazione valida trovata per: " + tagA + " + " + tagB);
    }

    public void DestroyPlants()
    {

       // Destroy;
         
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}

