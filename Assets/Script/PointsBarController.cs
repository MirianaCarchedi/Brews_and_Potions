using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsBarController : MonoBehaviour
{
    [Header("Fill Images")]
    public Image fillImage1;  // Barra nel primo Canvas
    public Image fillImage2;  // Barra nel secondo Canvas

    [Range(0f, 1f)]
    public float currentFill = 0f;
    private Coroutine fillRoutine;

    private void OnEnable()
    {
        // Sincronizza le barre con lo stato corrente dei punti quando il Canvas si attiva
        if (GameManager.instance != null)
        {
            float targetFill = (float)GameManager.instance.currentPoints / GameManager.instance.maxPoints;
            SetFillInstant(targetFill);
        }
    }

    // Aggiornamento graduale
    public void SetFill(float targetFill)
    {
        targetFill = Mathf.Clamp01(targetFill); // Assicura che sia tra 0 e 1

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillRoutine = StartCoroutine(FillOverTime(targetFill));
    }

    // Aggiornamento immediato
    public void SetFillInstant(float targetFill)
    {
        currentFill = Mathf.Clamp01(targetFill);
        if (fillImage1 != null)
            fillImage1.fillAmount = currentFill;
        if (fillImage2 != null)
            fillImage2.fillAmount = currentFill;
    }

    private IEnumerator FillOverTime(float targetFill)
    {
        float startFill = currentFill;
        float elapsed = 0f;
        float duration = 1f; // tempo per il riempimento graduale

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentFill = Mathf.Lerp(startFill, targetFill, elapsed / duration);

            if (fillImage1 != null)
                fillImage1.fillAmount = currentFill;
            if (fillImage2 != null)
                fillImage2.fillAmount = currentFill;

            yield return null;
        }

        currentFill = targetFill;
        if (fillImage1 != null)
            fillImage1.fillAmount = currentFill;
        if (fillImage2 != null)
            fillImage2.fillAmount = currentFill;
    }
}
