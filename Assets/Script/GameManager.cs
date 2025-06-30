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

    private bool hasStarted = false;

    public GameObject slot;                     // Lo slot dove avviene il drop

    // Tag richiesti per ogni personaggio
    public string requiredTagCharacter1 = "HolyStrenght";
    public string requiredTagCharacter2 = "MagicPower";
    public string requiredTagCharacter3 = "SpeedBoost";

    // Messaggi iniziali per ogni personaggio
    public string messageCharacter1;
    public string messageCharacter2;
    public string messageCharacter3;

    // Messaggi finali per ogni personaggio
    public string finalMessageCharacter1;
    public string finalMessageCharacter2;
    public string finalMessageCharacter3;

    // Messaggi negativi per ogni personaggio
    public string negativeMessageCharacter1;
    public string negativeMessageCharacter2;
    public string negativeMessageCharacter3;

    public Animator characterExitAnimator;      // Animator del personaggio che va via
    public GameObject nextCharacter;            // Il personaggio da attivare dopo
    public GameObject nextCharacter2;  // Il terzo personaggio da attivare dopo il secondo
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
        characterAnim.Play("Mario_Animation", 0);
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Mario_StandBy", 0);
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageCharacter1);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlaySequenceCharacter2(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);
        characterAnim.Play("Student_Animation", 0);
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Student_StandBy", 0);
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageCharacter2);
        yield return new WaitForSeconds(1f);
    }

    IEnumerator PlaySequenceCharacter3(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);
        characterAnim.Play("Lady_Animation", 0);
        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);
        characterAnim.Play("Lady_StandBy", 0);
        yield return new WaitForSeconds(1f);

        typingEffect.StartTyping(messageCharacter3);
        yield return new WaitForSeconds(1f);
    }


    public void OnButtonClicked()
    {
        if (slot.transform.childCount > 0)
        {
            GameObject droppedObject = slot.transform.GetChild(0).gameObject;

            string requiredTag = "";
            string finalMessage = "";
            string negativeMessage = "";

            if (currentCharacter.CompareTag("Character1"))
            {
                requiredTag = requiredTagCharacter1;
                finalMessage = finalMessageCharacter1;
                negativeMessage = negativeMessageCharacter1;
            }
            else if (currentCharacter.CompareTag("Character2"))
            {
                requiredTag = requiredTagCharacter2;
                finalMessage = finalMessageCharacter2;
                negativeMessage = negativeMessageCharacter2;
            }
            else if (currentCharacter.CompareTag("Character3"))
            {
                requiredTag = requiredTagCharacter3;
                finalMessage = finalMessageCharacter3;
                negativeMessage = negativeMessageCharacter3;
            }

            bool isCorrect = droppedObject.CompareTag(requiredTag);

            if (isCorrect)
            {
                StartCoroutine(HandleDropSequence(finalMessage, true));
            }
            else
            {
                typingEffect.StartTyping(negativeMessage);
                StartCoroutine(HandleDropSequence(negativeMessage, false));
            }
        }
        else
        {
            Debug.Log("Nessun oggetto presente nello slot.");
        }
    }

    public string nextSceneName;  // Nome della scena da caricare dopo il terzo personaggio

    IEnumerator HandleDropSequence(string message, bool isCorrect, int stage = 0)
    {
        typingEffect.StartTyping(message);

        yield return new WaitForSeconds(8f);

        if (characterExitAnimator != null)
        {
            characterExitAnimator.SetTrigger("Exit");
            yield return new WaitForSeconds(2.5f);
        }

        if (currentCharacter != null)
            currentCharacter.SetActive(false);

        if (stage == 0 && nextCharacter != null)
        {
            nextCharacter.SetActive(true);
            currentCharacter = nextCharacter;
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            Animator nextCharAnimator = currentCharacter.GetComponent<Animator>();
            if (nextCharAnimator != null)
            {
                if (currentCharacter.CompareTag("Character1"))
                    yield return StartCoroutine(PlaySequenceCharacter1(nextCharAnimator));
                else if (currentCharacter.CompareTag("Character2"))
                    yield return StartCoroutine(PlaySequenceCharacter2(nextCharAnimator));
                else if (currentCharacter.CompareTag("Character3"))
                    yield return StartCoroutine(PlaySequenceCharacter3(nextCharAnimator));
            }

            yield return StartCoroutine(HandleDropSequence(GetFinalMessageForCurrentCharacter(), true, 1));
        }
        else if (stage == 1 && nextCharacter2 != null)
        {
            nextCharacter2.SetActive(true);
            currentCharacter = nextCharacter2;
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            Animator nextCharAnimator = currentCharacter.GetComponent<Animator>();
            if (nextCharAnimator != null)
            {
                if (currentCharacter.CompareTag("Character1"))
                    yield return StartCoroutine(PlaySequenceCharacter1(nextCharAnimator));
                else if (currentCharacter.CompareTag("Character2"))
                    yield return StartCoroutine(PlaySequenceCharacter2(nextCharAnimator));
                else if (currentCharacter.CompareTag("Character3"))
                    yield return StartCoroutine(PlaySequenceCharacter3(nextCharAnimator));
            }

            // Dopo il terzo personaggio, stage 2 per uscita + cambio scena
            yield return StartCoroutine(HandleDropSequence(GetFinalMessageForCurrentCharacter(), true, 2));
        }
        else if (stage == 2)
        {
            // Uscita finale e cambio scena
            if (characterExitAnimator != null)
            {
                characterExitAnimator.SetTrigger("Exit");
                yield return new WaitForSeconds(2f);
            }

            if (currentCharacter != null)
                currentCharacter.SetActive(false);

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }



    // Funzione helper per prendere il messaggio finale corretto del personaggio attuale
    string GetFinalMessageForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return finalMessageCharacter1;
        else if (currentCharacter.CompareTag("Character2"))
            return finalMessageCharacter2;
        else if (currentCharacter.CompareTag("Character3"))
            return finalMessageCharacter3;
        return "";
    }

}


