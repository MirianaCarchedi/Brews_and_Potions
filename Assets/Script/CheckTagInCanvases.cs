using UnityEngine;
using System.Collections.Generic;

public class CheckTagInCanvases : MonoBehaviour
{
    [System.Serializable]
    public class TagChildPair
    {
        public string tag;
        public GameObject childToEnable;
    }

    public TagChildPair[] tagChildPairs;

    // Tiene traccia dei tag già "trovati" almeno una volta
    private static HashSet<string> seenTags = new HashSet<string>();

    private void OnEnable()
    {
        foreach (var pair in tagChildPairs)
        {
            // Se è già stato visto in passato, attiva direttamente il figlio
            if (seenTags.Contains(pair.tag))
            {
                pair.childToEnable?.SetActive(true);
                continue;
            }

            // Altrimenti, cerca il tag tra gli oggetti in scena (anche disattivati)
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(pair.tag) &&
                    obj.hideFlags == HideFlags.None &&
                    obj.scene.IsValid())
                {
                    seenTags.Add(pair.tag);
                    pair.childToEnable?.SetActive(true);
                    break;
                }
            }
        }
    }
}


