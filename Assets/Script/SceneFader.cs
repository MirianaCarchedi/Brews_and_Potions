using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class SceneFader : MonoBehaviour
{
    public Canvas Laboratorio;
    public Canvas Bancone;

    public Image sfondoNeroLab;
    public Image sfondoNeroBancone;

    public Animator animatorLab;
    public Animator animatorBancone;

    public void FadeToSceneLab()
    {
        StartCoroutine(FadeAndSwitchCanvas(
            canvasToShow: Laboratorio,
            canvasToHide: Bancone,
            fadeImage: sfondoNeroLab,
            animator: animatorLab
        ));
    }

    public void FadeToSceneBancone()
    {
        StartCoroutine(FadeAndSwitchCanvas(
            canvasToShow: Bancone,
            canvasToHide: Laboratorio,
            fadeImage: sfondoNeroBancone,
            animator: animatorBancone
        ));
    }

    IEnumerator FadeAndSwitchCanvas(Canvas canvasToShow, Canvas canvasToHide, Image fadeImage, Animator animator)
    {
        //  Attiva solo lo sfondo nero (non il canvas intero)
        fadeImage.gameObject.SetActive(true);

        //  Ora il GameObject è attivo, quindi puoi riprodurre
        animator.Play("FadeIn_Animation");

        yield return new WaitForSeconds(1f);

        // Ora cambia canvas
        canvasToShow.gameObject.SetActive(true);
        canvasToHide.gameObject.SetActive(false);

        animator.Play("FadeOut_Animation");

        yield return new WaitForSeconds(1f);

        fadeImage.gameObject.SetActive(false);
    }

}


