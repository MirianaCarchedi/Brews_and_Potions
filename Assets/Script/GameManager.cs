using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator bubbleAnimator;           // Animator della nuvoletta
    public Animator fadeOut_Animator;

    public TypingEffect typingEffect;         // Script che gestisce la scrittura

    public Image sfondoNero;
    public string messageToDisplay;

    private bool hasStarted = false;

    public GameObject slot;                     // Lo slot dove avviene il drop

    // Tag richiesti per ogni personaggio
    public string requiredTagCharacter1 = "HolyStrenght";
    public string requiredTagCharacter2 = "MagicPower";
    public string requiredTagCharacter3 = "SpeedBoost";

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
            if (currentCharacter != null)
            {
                Animator currentAnimator = currentCharacter.GetComponent<Animator>();
                if (currentAnimator != null)
                {
                    if (currentCharacter.CompareTag("Character1"))
                        StartCoroutine(PlaySequenceCharacter1(currentAnimator));
                    else if (currentCharacter.CompareTag("Character2"))
                        StartCoroutine(PlaySequenceCharacter2(currentAnimator));
                    else if (currentCharacter.CompareTag("Character3"))
                        StartCoroutine(PlaySequenceCharacter3(currentAnimator));
                }
            }
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

    IEnumerator PlaySequenceCharacter1(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);
        characterAnim.Play("Mario_Animation");
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Mario_StandBy");
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageToDisplay);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlaySequenceCharacter2(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);
        characterAnim.Play("Student_Animation");
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Student_StandBy");
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageToDisplay);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlaySequenceCharacter3(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);
        characterAnim.Play("Lady_Animation");
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Lady_StandBy");
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageToDisplay);
        yield return new WaitForSeconds(1f);
    }

    public void OnButtonClicked()
    {
        if (slot.transform.childCount > 0)
        {
            GameObject droppedObject = slot.transform.GetChild(0).gameObject;

            string requiredTag = "";

            if (currentCharacter.CompareTag("Character1"))
                requiredTag = requiredTagCharacter1;
            else if (currentCharacter.CompareTag("Character2"))
                requiredTag = requiredTagCharacter2;
            else if (currentCharacter.CompareTag("Character3"))
                requiredTag = requiredTagCharacter3;

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
        {
            nextCharacter.SetActive(true);
            Animator nextCharAnimator = nextCharacter.GetComponent<Animator>();
            if (nextCharAnimator != null)
            {
                if (nextCharacter.CompareTag("Character1"))
                    yield return StartCoroutine(PlaySequenceCharacter1(nextCharAnimator));
                else if (nextCharacter.CompareTag("Character2"))
                    yield return StartCoroutine(PlaySequenceCharacter2(nextCharAnimator));
                else if (nextCharacter.CompareTag("Character3"))
                    yield return StartCoroutine(PlaySequenceCharacter3(nextCharAnimator));
            }
        }
    }
}

