using UnityEngine;

public class CheckTagInCanvases : MonoBehaviour
{
    [System.Serializable]
    public class TagChildPair
    {
        public string tag;
        public GameObject childToEnable;
    }

    public TagChildPair[] tagChildPairs;

    private void OnEnable()
    {
        foreach (var pair in tagChildPairs)
        {
            // Disattiva il figlio all'inizio
            if (pair.childToEnable != null)
                pair.childToEnable.SetActive(false);

            // Trova TUTTI gli oggetti della scena (anche disattivi)
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                // ? Filtra SOLO quelli della scena (esclude prefab negli asset)
                if (obj.CompareTag(pair.tag) &&
                    obj.hideFlags == HideFlags.None &&
                    obj.scene.IsValid()) // <-- importante!
                {
                    pair.childToEnable?.SetActive(true);
                    break; // trovato, basta
                }
            }
        }
    }
}

