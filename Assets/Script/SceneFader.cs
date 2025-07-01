using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public GameObject laboratorio;
    public GameObject bancone;

    public Image sfondoNero;
    public Animator fadeAnimator;

    public void FadeToLab()
    {
        StartCoroutine(FadeAndSwitch(laboratorio, bancone));
    }

    public void FadeToBancone()
    {
        StartCoroutine(FadeAndSwitch(bancone, laboratorio));
    }

    IEnumerator FadeAndSwitch(GameObject canvasToShow, GameObject canvasToHide)
    {
        sfondoNero.gameObject.SetActive(true);

        // Avvia il fade out
        fadeAnimator.SetTrigger("FadeIn_Animation");

        yield return new WaitForSeconds(2f); // attesa del fade out

        // Cambia canvas
        canvasToShow.SetActive(true);
        canvasToHide.SetActive(false);

        // Avvia il fade in
        fadeAnimator.SetTrigger("FadeOut_Animation");

        yield return new WaitForSeconds(2f); // attesa del fade in

        sfondoNero.gameObject.SetActive(false);
    }
}

