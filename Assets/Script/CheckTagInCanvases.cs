using UnityEngine;
using System.Collections.Generic;

public class CheckTagInCanvases : MonoBehaviour
{
    [System.Serializable]
    public class TagChildPair
    {
        public string tag;
        public GameObject childToEnable; // il popup associato
    }

    public TagChildPair[] tagChildPairs;

    // Tiene traccia dei tag già attivati almeno una volta
    private static HashSet<string> seenTags = new HashSet<string>();

    public Canvas targetCanvas; // Canvas da monitorare

    private void Update()
    {
        foreach (var pair in tagChildPairs)
        {
            if (seenTags.Contains(pair.tag))
            {
                // Se il popup è già stato attivato una volta, lo lascia attivo
                if (pair.childToEnable != null && !pair.childToEnable.activeSelf)
                    pair.childToEnable.SetActive(true);
                continue;
            }

            // Cerca tra tutti i figli del canvas (anche disattivati)
            Transform[] children = targetCanvas.GetComponentsInChildren<Transform>(true);
            foreach (var child in children)
            {
                if (child == null) continue;
                if (child.CompareTag(pair.tag) && child.gameObject.activeInHierarchy)
                {
                    seenTags.Add(pair.tag);
                    pair.childToEnable?.SetActive(true);
                    break;
                }
            }
        }
    }
}
