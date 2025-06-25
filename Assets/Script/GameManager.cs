using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: MonoBehaviour

{
    public Animator characterAnimator;        // Animator del personaggio
    public Animator bubbleAnimator;           // Animator della nuvoletta
    public Animator buttonAnimator;
    public Animator ArrowAnimator;
    public Animator fadeOut_Animator;

    public TypingEffect typingEffect;         // Script che gestisce la scrittura

    public Image sfondoNero;
    public string messageToDisplay;
    public string nextMessage;
    public string thirdMessage;

    private int clickCount = 0;

    void Start()
    {

        Time.timeScale = 1f;
        StartCoroutine(FadeIn());

        if (SceneManager.GetActiveScene().name == "Prova_Bancone")
        {
            StartCoroutine(PlaySequence());
        }
            
    }
    IEnumerator FadeIn()
    {
        fadeOut_Animator.Play("FadeOut_Animation");
        yield return new WaitForSeconds(1f);
        sfondoNero.gameObject.SetActive(false);
    }

    IEnumerator PlaySequence()
    {
        
        yield return new WaitForSeconds(1f);
        // 1. Avvia animazione del personaggio
        characterAnimator.SetTrigger("");
        yield return new WaitForSeconds(2f); // sostituisci con la durata reale

        // 2. Avvia animazione della nuvoletta
        yield return StartCoroutine(PlayBubbleAnimation());

        // 3. Avvia scrittura a macchina
        typingEffect.StartTyping(messageToDisplay);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PlayButtonAnimation());
    }

    
    IEnumerator PlayBubbleAnimation()
    {
        bubbleAnimator.gameObject.SetActive(true); // attiva la nuvola se è disattivata
        bubbleAnimator.SetTrigger("");
        yield return new WaitForSeconds(1f); // sostituisci con la durata reale dell’animazione
    }

    IEnumerator PlayButtonAnimation()
    {
        buttonAnimator.gameObject.SetActive(true);
        buttonAnimator.SetTrigger("");
        yield return new WaitForSeconds(0f);
    }
    IEnumerator PlayArrowAnimation()
    {
        buttonAnimator.gameObject.SetActive(true);
        buttonAnimator.SetTrigger("");
        yield return new WaitForSeconds(0f);
    }
    public void OnButtonClicked()
    {
        clickCount++;

        if (clickCount == 1)
        {
            typingEffect.StartTyping(nextMessage);
        }
        else if (clickCount == 2)
        {
            typingEffect.StartTyping(thirdMessage);
            ArrowAnimator.gameObject.SetActive(true);

        }
    }


}


