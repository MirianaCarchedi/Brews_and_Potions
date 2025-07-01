using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public GameObject laboratorioCanvas;
    public GameObject banconeCanvas;

    public GameObject sfondoNeroCanvas;  // Canvas che contiene lo sfondo nero e l'animator
    public Animator fadeAnimator;

    private void Start()
    {

    }

    public void FadeToLab()
    {
        StartCoroutine(FadeAndSwitch(laboratorioCanvas, banconeCanvas));
    }

    public void FadeToBancone()
    {
        StartCoroutine(FadeAndSwitch(banconeCanvas, laboratorioCanvas));
    }

    private IEnumerator FadeAndSwitch(GameObject canvasToShow, GameObject canvasToHide)
    {
        sfondoNeroCanvas.SetActive(true);

        // Avvia animazione fade-in (fade to nero)
        fadeAnimator.Play("FadeIn_Animation");

        // Aspetta che finisca l'animazione (es. 2 secondi)
        yield return new WaitForSeconds(2f);

        // Cambia canvas
        canvasToHide.SetActive(false);
        canvasToShow.SetActive(true);

        // Avvia animazione fade-out (fade da nero a trasparente)
        fadeAnimator.Play("FadeOut_Animation");

        yield return new WaitForSeconds(2f);

        sfondoNeroCanvas.SetActive(false);
    }
}


