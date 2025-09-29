using UnityEngine;

public class AutoReactivate : MonoBehaviour
{
    public float reactivateTime = 9f;
    public GameObject primo;

    void OnDisable()
    {
        // Avvia la coroutine quando l'oggetto viene disattivato
        StartCoroutine(ReactivateAfterTime());
    }

    private System.Collections.IEnumerator ReactivateAfterTime()
    {
        yield return new WaitForSeconds(reactivateTime);
        primo.SetActive(true);
    }
}
