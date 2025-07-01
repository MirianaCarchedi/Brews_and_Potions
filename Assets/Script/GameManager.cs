using System.Collections;
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

    public GameObject slot;                   // Lo slot dove avviene il drop

    // Nuovi riferimenti diretti ai GameObject da attivare per Character 2 e 3
    public GameObject objectForCharacter2;
    public GameObject objectForCharacter3;
    public Transform slotToShowObject;        // Slot dove mettere questi oggetti

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
    public GameObject nextCharacter2;           // Il terzo personaggio da attivare dopo il secondo
    public GameObject currentCharacter;         // Il personaggio attuale

    private int currentStage = 0;                // Stage corrente (0,1,2)
    private bool waitingForNextClick = false;    // Se si aspetta il click per passare al prossimo personaggio

    public string nextSceneName;  // Nome della scena da caricare dopo il terzo personaggio

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

    IEnumerator PlaySequenceForCurrentCharacter(Animator characterAnim)
    {
        yield return new WaitForSeconds(1f);

        if (currentCharacter.CompareTag("Character1"))
            characterAnim.Play("Mario_Animation", 0);
        else if (currentCharacter.CompareTag("Character2"))
            characterAnim.Play("Student_Animation", 0);
        else if (currentCharacter.CompareTag("Character3"))
            characterAnim.Play("Lady_Animation", 0);

        yield return new WaitForSeconds(2f);

        bubbleAnimator.gameObject.SetActive(true);

        if (currentCharacter.CompareTag("Character1"))
            characterAnim.Play("Mario_StandBy", 0);
        else if (currentCharacter.CompareTag("Character2"))
            characterAnim.Play("Student_StandBy", 0);
        else if (currentCharacter.CompareTag("Character3"))
            characterAnim.Play("Lady_StandBy", 0);

        yield return new WaitForSeconds(1f);

        // Messaggio iniziale
        typingEffect.StartTyping(GetInitialMessageForCurrentCharacter());
        yield return null;
    }

    public void OnButtonClicked()
    {
        if (!waitingForNextClick)
        {
            // Primo click: verifica pozione, distruggi l'oggetto nello slot, mostra messaggio finale o negativo, poi aspetta il prossimo click
            if (slot.transform.childCount > 0)
            {
                GameObject droppedObject = slot.transform.GetChild(0).gameObject;

                // Distruggi la pozione appena cliccato
                Destroy(droppedObject);

                string requiredTag = GetRequiredTagForCurrentCharacter();
                string messageToShow;
                bool isCorrect = droppedObject.CompareTag(requiredTag);

                if (isCorrect)
                    messageToShow = GetFinalMessageForCurrentCharacter();
                else
                    messageToShow = GetNegativeMessageForCurrentCharacter();

                typingEffect.StartTyping(messageToShow);
                waitingForNextClick = true;
            }
            else
            {
                Debug.Log("Nessun oggetto presente nello slot.");
            }
        }
        else
        {
            // Secondo click: fai uscire il personaggio e passa al prossimo
            StartCoroutine(ExitCurrentAndAdvance());
            waitingForNextClick = false;
        }
    }


    IEnumerator ExitCurrentAndAdvance()
    {
        if (characterExitAnimator != null)
        {
            // Partendo dall'animazione di uscita del personaggio corrente (diversa per ciascuno)
            if (currentCharacter.CompareTag("Character1"))
                characterExitAnimator.Play("Mario_Exit", 0);
            else if (currentCharacter.CompareTag("Character2"))
                characterExitAnimator.Play("Student_Exit", 0);
            else if (currentCharacter.CompareTag("Character3"))
                characterExitAnimator.Play("Lady_Exit", 0);

            yield return new WaitForSeconds(2.5f);
        }

        if (currentCharacter != null)
            currentCharacter.SetActive(false);

        currentStage++;

        if (currentStage == 1 && nextCharacter != null)
        {
            currentCharacter = nextCharacter;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            // Mostra l'oggetto per Character 2
            ShowObjectInSlot(objectForCharacter2);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage == 2 && nextCharacter2 != null)
        {
            currentCharacter = nextCharacter2;
            currentCharacter.SetActive(true);
            characterExitAnimator = currentCharacter.GetComponent<Animator>();

            // Mostra l'oggetto per Character 3
            ShowObjectInSlot(objectForCharacter3);

            Animator nextAnim = currentCharacter.GetComponent<Animator>();
            if (nextAnim != null)
                yield return StartCoroutine(PlaySequenceForCurrentCharacter(nextAnim));
        }
        else if (currentStage >= 3)
        {
            // Fine sequenza: uscita finale e cambio scena
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

    // Metodo helper per mostrare l'oggetto nello slot e attivarlo
    void ShowObjectInSlot(GameObject prefab)
    {
        if (prefab != null && slotToShowObject != null)
        {
            // Istanzia il prefab nella scena
            GameObject instance = Instantiate(prefab, slotToShowObject);
            instance.SetActive(true);
        }
    }


    // Helper per prendere i messaggi e tag in base al personaggio attuale
    string GetRequiredTagForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return "HolyStrenght";
        else if (currentCharacter.CompareTag("Character2"))
            return "LightMind";
        else if (currentCharacter.CompareTag("Character3"))
            return "OpenHeart";
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
        return "";
    }
}

