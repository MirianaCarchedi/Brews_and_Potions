using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator bubbleAnimator;           // Animator della nuvoletta
    public Animator fadeOut_Animator;

    public TypingEffect typingEffect;         // Script che gestisce la scrittura
    public TMPro.TextMeshProUGUI textComponent;


    public GameObject sfondoNeroCanvas;

    private bool hasStarted = false;

    public GameObject slot;                   // Lo slot dove avviene il drop

    // Riferimenti diretti agli oggetti dei personaggi
    public GameObject objectForCharacter2;
    public GameObject objectForCharacter3;
    public GameObject objectForCharacter4;
    public GameObject objectForCharacter5;
    public Transform slotToShowObject;        // Slot dove mettere questi oggetti

    // Messaggi iniziali per ogni personaggio
    public string messageCharacter1;
    public string messageCharacter2;
    public string messageCharacter3;
    public string messageCharacter4;
    public string messageCharacter5;

    // Messaggi finali per ogni personaggio
    public string finalMessageCharacter1;
    public string finalMessageCharacter2;
    public string finalMessageCharacter3;
    public string finalMessageCharacter4;
    public string finalMessageCharacter5;

    // Messaggi negativi per ogni personaggio
    public string negativeMessageCharacter1;
    public string negativeMessageCharacter2;
    public string negativeMessageCharacter3;
    public string negativeMessageCharacter4;
    public string negativeMessageCharacter5;

    public Animator characterExitAnimator;      // Animator del personaggio che va via
    public GameObject nextCharacter;            // Personaggio 2
    public GameObject nextCharacter2;           // Personaggio 3
    public GameObject nextCharacter3;           // Personaggio 4
    public GameObject nextCharacter4;           // Personaggio 5
    public GameObject currentCharacter;         // Personaggio attuale

    private int currentStage = 0;                // Stage corrente (0,1,2,3,4)
    private bool waitingForNextClick = false;    // Se si aspetta il click per passare al prossimo personaggio

    public string nextSceneName;  // Nome della scena da caricare dopo il quinto personaggio

    public AudioSource audioSource;   // Riferimento all'AudioSource
    public AudioClip soundEffect;     // Il suono da riprodurre
    public GameObject advanceButton; // Bottone che fa avanzare il personaggio



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
                    StartCoroutine(PlaySequenceForCurrentCharacter(currentAnimator));
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

        if (sfondoNeroCanvas != null)
        {
            sfondoNeroCanvas.SetActive(false);
        }

        hasStarted = false;
    }

    IEnumerator FadeIn()
    {
        fadeOut_Animator.Play("FadeOut_Animation");
        yield return new WaitForSeconds(2f);
        sfondoNeroCanvas.SetActive(false);
    }

    IEnumerator PlaySequenceForCurrentCharacter(Animator characterAnim)
    {
        yield return new WaitForSeconds(0f);
        audioSource.PlayOneShot(soundEffect);

        // Avvia animazione principale
        if (currentCharacter.CompareTag("Character1"))
            characterAnim.Play("Mario_Animation", 0);
        else if (currentCharacter.CompareTag("Character2"))
            characterAnim.Play("Student_Animation", 0);
        else if (currentCharacter.CompareTag("Character3"))
            characterAnim.Play("Lady_Animation", 0);
        else if (currentCharacter.CompareTag("Character4"))
            characterAnim.Play("Artist_Animation", 0);
        else if (currentCharacter.CompareTag("Character5"))
            characterAnim.Play("Traveler_Animation", 0);

        yield return new WaitForSeconds(2f);

        // Avvia coroutine StandBy
        StartCoroutine(PlayStandByAnimation(characterAnim));

        // Continua con il resto del flusso
        bubbleAnimator.gameObject.SetActive(true);
        typingEffect.StartTyping(GetInitialMessageForCurrentCharacter());
    }

    IEnumerator PlayStandByAnimation(Animator characterAnim)
    {
        yield return new WaitForSeconds(3.5f);

        if (currentCharacter.CompareTag("Character1"))
            characterAnim.Play("Mario_StandBy", 0);
        else if (currentCharacter.CompareTag("Character2"))
            characterAnim.Play("Student_StandBy", 0);
        else if (currentCharacter.CompareTag("Character3"))
            characterAnim.Play("Lady_StandBy", 0);
        else if (currentCharacter.CompareTag("Character4"))
            characterAnim.Play("Artist_StandBy", 0);
        else if (currentCharacter.CompareTag("Character5"))
            characterAnim.Play("Traveler_StandBy", 0);

        yield return null;
    }


    public void OnButtonClicked()
    {
        if (slot.transform.childCount > 0)
        {
            GameObject droppedObject = slot.transform.GetChild(0).gameObject;

            droppedObject.transform.SetParent(null);
            droppedObject.SetActive(false);

            string requiredTag = GetRequiredTagForCurrentCharacter();
            string messageToShow;
            bool isCorrect = droppedObject.CompareTag(requiredTag);

            if (isCorrect)
                messageToShow = GetFinalMessageForCurrentCharacter();
            else
                messageToShow = GetNegativeMessageForCurrentCharacter();

            typingEffect.StartTyping(messageToShow);

            if (advanceButton != null)
                advanceButton.SetActive(true);
        }
        else
        {
            Debug.Log("Nessun oggetto presente nello slot.");
        }
    }

    public void OnAdvanceButtonClicked()
    {
        StartCoroutine(ExitCurrentAndAdvance());

        if (advanceButton != null)
            advanceButton.SetActive(false);
    }


    IEnumerator ExitCurrentAndAdvance()
    {
        if (characterExitAnimator != null)
        {
            if (currentCharacter.CompareTag("Character1"))
                characterExitAnimator.Play("Mario_Exit", 0);
            else if (currentCharacter.CompareTag("Character2"))
                characterExitAnimator.Play("Student_Exit", 0);
            else if (currentCharacter.CompareTag("Character3"))
                characterExitAnimator.Play("Lady_Exit", 0);
            else if (currentCharacter.CompareTag("Character4"))
                characterExitAnimator.Play("Artist_Exit", 0);
            else if (currentCharacter.CompareTag("Character5"))
                characterExitAnimator.Play("Traveler_Exit", 0);

            yield return new WaitForSeconds(3f);
        }

        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
            textComponent.text = "";
        }

        currentStage++;

        if (currentStage == 1 && nextCharacter != null)
        {
            currentCharacter = nextCharacter;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            if (objectForCharacter2 != null)
                objectForCharacter2.SetActive(true);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage == 2 && nextCharacter2 != null)
        {
            currentCharacter = nextCharacter2;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            if (objectForCharacter3 != null)
                objectForCharacter3.SetActive(true);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage == 3 && nextCharacter3 != null)
        {
            currentCharacter = nextCharacter3;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            if (objectForCharacter4 != null)
                objectForCharacter4.SetActive(true);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage == 4 && nextCharacter4 != null)
        {
            currentCharacter = nextCharacter4;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            if (objectForCharacter5 != null)
                objectForCharacter5.SetActive(true);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage >= 5)
        {
            if (characterExitAnimator != null)
            {
                if (currentCharacter != null)
                {
                    if (currentCharacter.CompareTag("Character1"))
                        characterExitAnimator.Play("Mario_Exit", 0);
                    else if (currentCharacter.CompareTag("Character2"))
                        characterExitAnimator.Play("Student_Exit", 0);
                    else if (currentCharacter.CompareTag("Character3"))
                        characterExitAnimator.Play("Lady_Exit", 0);
                    else if (currentCharacter.CompareTag("Character4"))
                        characterExitAnimator.Play("Artist_Exit", 0);
                    else if (currentCharacter.CompareTag("Character5"))
                        characterExitAnimator.Play("Traveler_Exit", 0);

                    yield return new WaitForSeconds(2f);
                }
            }

            if (currentCharacter != null)
                currentCharacter.SetActive(false);

            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }


    void ShowObjectInSlot(GameObject prefab)
    {
        if (prefab != null && slotToShowObject != null)
        {
            GameObject instance = Instantiate(prefab, slotToShowObject);
            instance.SetActive(true);
        }
    }


    string GetRequiredTagForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return "HolyStrenght";
        else if (currentCharacter.CompareTag("Character2"))
            return "LightMind";
        else if (currentCharacter.CompareTag("Character3"))
            return "OpenHearth";
        else if (currentCharacter.CompareTag("Character4"))
            return "SilentCalm";
        else if (currentCharacter.CompareTag("Character5"))
            return "GuardianBrew";
        return "";
    }

    string GetInitialMessageForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return messageCharacter1;
        else if (currentCharacter.CompareTag("Character2"))
            return messageCharacter2;
        else if (currentCharacter.CompareTag("Character3"))
            return messageCharacter3;
        else if (currentCharacter.CompareTag("Character4"))
            return messageCharacter4;
        else if (currentCharacter.CompareTag("Character5"))
            return messageCharacter5;
        return "";
    }

    string GetFinalMessageForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return finalMessageCharacter1;
        else if (currentCharacter.CompareTag("Character2"))
            return finalMessageCharacter2;
        else if (currentCharacter.CompareTag("Character3"))
            return finalMessageCharacter3;
        else if (currentCharacter.CompareTag("Character4"))
            return finalMessageCharacter4;
        else if (currentCharacter.CompareTag("Character5"))
            return finalMessageCharacter5;
        return "";
    }

    string GetNegativeMessageForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return negativeMessageCharacter1;
        else if (currentCharacter.CompareTag("Character2"))
            return negativeMessageCharacter2;
        else if (currentCharacter.CompareTag("Character3"))
            return negativeMessageCharacter3;
        else if (currentCharacter.CompareTag("Character4"))
            return negativeMessageCharacter4;
        else if (currentCharacter.CompareTag("Character5"))
            return negativeMessageCharacter5;
        return "";
    }
}
