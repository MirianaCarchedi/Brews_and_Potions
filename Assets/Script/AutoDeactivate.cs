using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    // Tempo in secondi prima della disattivazione
    public float deactivateTime = 6f;
    public GameObject Panel;

    void OnEnable()
    {
        // Avvia la coroutine quando l'oggetto viene attivato
        StartCoroutine(DeactivateAfterTime());
    }

    private System.Collections.IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(deactivateTime);
        gameObject.SetActive(false);

    }
}
