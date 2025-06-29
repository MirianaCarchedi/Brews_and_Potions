using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator characterAnimator;        // Animator del personaggio
    public Animator bubbleAnimator;           // Animator della nuvoletta
    public Animator fadeOut_Animator;

    public TypingEffect typingEffect;         // Script che gestisce la scrittura

    public Image sfondoNero;
    public string messageToDisplay;

    private bool hasStarted = false;

    public GameObject slot;                     // Lo slot dove avviene il drop
    public string requiredTag = "HolyStrenght"; // Il tag richiesto per il drop
    public string finalMessage;

    public Animator characterExitAnimator;      // Animator del personaggio che va via
    public GameObject nextCharacter;            // Il personaggio da attivare dopo
    public GameObject currentCharacter;         // Il personaggio attuale

    void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(FadeIn());
    }

    void OnEnable()
    {
        if (!hasStarted)
        {
            StartCoroutine(PlaySequence());
            hasStarted = true;
        }
    }

    void OnDisable()
    {
        if (bubbleAnimator != null)
        {
            bubbleAnimator.gameObject.SetActive(false);
            bubbleAnimator.Rebind();
            bubbleAnimator.Update(0f);
        }

        if (sfondoNero != null)
        {
            sfondoNero.gameObject.SetActive(true);
        }

        hasStarted = false;
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
        characterAnimator.Play("Mario_Animation");
        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(PlayBubbleAnimation());
        typingEffect.StartTyping(messageToDisplay);
        yield return new WaitForSeconds(1f);

    }

    IEnumerator PlayBubbleAnimation()
    {
        bubbleAnimator.gameObject.SetActive(true);
        characterAnimator.Play("Mario_StandBy");
        yield return new WaitForSeconds(1f);
    }


    public void OnButtonClicked()
    {
            if (slot.transform.childCount > 0)
            {
                GameObject droppedObject = slot.transform.GetChild(0).gameObject;

                if (droppedObject.CompareTag(requiredTag))
                {
                    StartCoroutine(HandleCorrectDropSequence());
                }
                else
                {
                    Debug.Log("Oggetto nel slot ha un tag sbagliato.");
                }
            }
            else
            {
                Debug.Log("Nessun oggetto presente nello slot.");
            }
        
    }

    IEnumerator HandleCorrectDropSequence()
    {
        typingEffect.StartTyping(finalMessage);

        yield return new WaitForSeconds(2.5f);

        if (characterExitAnimator != null)
        {
            characterExitAnimator.SetTrigger("Exit");
            yield return new WaitForSeconds(2f);
        }

        if (currentCharacter != null)
            currentCharacter.SetActive(false);

        if (nextCharacter != null)
            nextCharacter.SetActive(true);
    }
}

