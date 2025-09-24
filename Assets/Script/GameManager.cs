using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator bubbleAnimator;
    public Animator fadeOut_Animator;

    public TypingEffect typingEffect;
    public TMPro.TextMeshProUGUI textComponent;

    public GameObject sfondoNeroCanvas;

    private bool hasStarted = false;

    public GameObject slot;

    public GameObject objectForCharacter2;
    public GameObject objectForCharacter3;
    public GameObject objectForCharacter4;
    public GameObject objectForCharacter5;
    public Transform slotToShowObject;

    public string messageCharacter1;
    public string messageCharacter2;
    public string messageCharacter3;
    public string messageCharacter4;
    public string messageCharacter5;

    public string finalMessageCharacter1;
    public string finalMessageCharacter2;
    public string finalMessageCharacter3;
    public string finalMessageCharacter4;
    public string finalMessageCharacter5;

    public string negativeMessageCharacter1;
    public string negativeMessageCharacter2;
    public string negativeMessageCharacter3;
    public string negativeMessageCharacter4;
    public string negativeMessageCharacter5;

    public Animator characterExitAnimator;
    public GameObject nextCharacter;
    public GameObject nextCharacter2;
    public GameObject nextCharacter3;
    public GameObject nextCharacter4;
    public GameObject currentCharacter;

    [Header("Pop-up intermezzo per ogni personaggio")]
    public GameObject popupIntermezzoCharacter1;
    public GameObject popupIntermezzoCharacter2;
    public GameObject popupIntermezzoCharacter3;
    public GameObject popupIntermezzoCharacter4;

    private int currentStage = 0;
    private bool waitingForNextClick = false;

    public string nextSceneName;

    public AudioSource audioSource;
    public AudioClip soundEffect;
    public GameObject advanceButton;

    [Header("Sistema Punti")]
    [Header("Gestione Punti")]
    [Header("Barre Punti")]
    public PointsBarController pointsBarController1;
    public PointsBarController pointsBarController2;
    public int maxPoints = 60;
    public int currentPoints = 0;
    public UnityEngine.UI.Image pointsBar;  // UI Image con Fill Type: Horizontal
    private const int minPoints = 0;

    public static GameManager instance;

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

        StartCoroutine(PlayStandByAnimation(characterAnim));

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

            string messageToShow;

            string[] requiredTags = GetRequiredTagForCurrentCharacter();
            bool isCorrect = false;

            foreach (string tag in requiredTags)
            {
                if (droppedObject.CompareTag(tag))
                {
                    isCorrect = true;
                    break;
                }
            }



            if (isCorrect)
            {
                messageToShow = GetFinalMessageForCurrentCharacter();
                // Controlla se l'oggetto o uno dei figli ha il componente PerfectPoint
                bool hasPerfect = droppedObject.GetComponentInChildren<PerfectPoint>() != null;

                if (hasPerfect)
                    currentPoints += 10;
                else
                    currentPoints += 5;

            }
            else
            {
                messageToShow = GetNegativeMessageForCurrentCharacter();

                // Sottrae 3 punti
                currentPoints -= 3;
            }

            // Limita i punti tra 0 e maxPoints
            currentPoints = Mathf.Clamp(currentPoints, 0, maxPoints);

            // Aggiorna entrambe le barre
            float fillAmount = (float)currentPoints / maxPoints;

            if (pointsBarController1 != null)
                pointsBarController1.SetFill(fillAmount);

            if (pointsBarController2 != null)
                pointsBarController2.SetFill(fillAmount);

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

        GameObject intermezzoPopup = null;
        if (currentStage == 1) intermezzoPopup = popupIntermezzoCharacter1;
        else if (currentStage == 2) intermezzoPopup = popupIntermezzoCharacter2;
        else if (currentStage == 3) intermezzoPopup = popupIntermezzoCharacter3;
        else if (currentStage == 4) intermezzoPopup = popupIntermezzoCharacter4;

        if (intermezzoPopup != null)
            yield return StartCoroutine(ShowIntermezzoPopup(intermezzoPopup));

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

    private IEnumerator ShowIntermezzoPopup(GameObject popup)
    {
        if (popup != null)
        {
            popup.SetActive(true);
            yield return new WaitForSeconds(4f);
            Destroy(popup);
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

    string[] GetRequiredTagForCurrentCharacter()
    {
        if (currentCharacter.CompareTag("Character1"))
            return new string[] { "HolyStrength" };
        else if (currentCharacter.CompareTag("Character2"))
            return new string[] { "LightMind" };
        else if (currentCharacter.CompareTag("Character3"))
            return new string[] { "OpenHearth", "StrongerSelf" }; // due tag validi
        else if (currentCharacter.CompareTag("Character4"))
            return new string[] { "LimpidVision" };
        else if (currentCharacter.CompareTag("Character5"))
            return new string[] { "GuardianBrew", "Pathfinder" }; // due tag validi
        return new string[0];
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
