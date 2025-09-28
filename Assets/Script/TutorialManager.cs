using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [System.Serializable]
    public class TutorialStep
    {
        [TextArea] public string popupText;
        public GameObject popupCanvas;
        public Button nextButton;       
        public float autoHideTime = 0f; 
        public float delayBeforeNextStep = 0f; 
    }

    [Header("Lista completa degli step del tutorial")]
    public List<TutorialStep> steps = new List<TutorialStep>();

    [Header("Audio da silenziare durante il tutorial")]
    public List<AudioSource> audioSourcesToMute = new List<AudioSource>();

    private int currentStepIndex = 0;
    private bool tutorialRunning = false;

    void Start()
    {
        StartTutorial();
    }

    public void StartTutorial()
    {
        if (steps.Count == 0) return;

        tutorialRunning = true;
        currentStepIndex = 0;

        // Blocca tempo e audio
        Time.timeScale = 0f;
        foreach (var aud in audioSourcesToMute)
            aud.Pause();

        ShowStep(currentStepIndex, true);
    }

    void ShowStep(int index, bool isFirstStep = false)
    {
        if (index < 0 || index >= steps.Count) return;

        // Nasconde tutti i popup
        foreach (var step in steps)
        {
            if (step.popupCanvas != null)
                step.popupCanvas.SetActive(false);
        }

        currentStepIndex = index;
        var stepToShow = steps[index];

        if (stepToShow.popupCanvas != null)
            stepToShow.popupCanvas.SetActive(true);

        // Configura pulsante next
        if (stepToShow.nextButton != null)
        {
            stepToShow.nextButton.onClick.RemoveAllListeners();
            stepToShow.nextButton.onClick.AddListener(() =>
            {
                // Disattiva popup ricorrente
                if (stepToShow.popupCanvas != null)
                    stepToShow.popupCanvas.SetActive(false);

                // Ripristina tempo e audio se è il primo step
                if (isFirstStep)
                {
                    Time.timeScale = 1f;
                    foreach (var aud in audioSourcesToMute)
                        aud.UnPause();
                }

                StartCoroutine(NextStepWithDelay(stepToShow.delayBeforeNextStep));
            });
        }

        // Gestione auto-hide se impostato
        if (!isFirstStep && stepToShow.autoHideTime > 0)
        {
            StartCoroutine(AutoHideStep(stepToShow));
        }
    }

    IEnumerator AutoHideStep(TutorialStep step)
    {
        yield return new WaitForSecondsRealtime(step.autoHideTime);

        if (step.popupCanvas != null)
            step.popupCanvas.SetActive(false);

        yield return StartCoroutine(NextStepWithDelay(step.delayBeforeNextStep));
    }

    IEnumerator NextStepWithDelay(float delay)
    {
        if (delay > 0)
            yield return new WaitForSecondsRealtime(delay);

        int nextIndex = currentStepIndex + 1;
        if (nextIndex < steps.Count)
        {
            ShowStep(nextIndex);
        }
        else
        {
            EndTutorial();
        }
    }

    void EndTutorial()
    {
        foreach (var step in steps)
        {
            if (step.popupCanvas != null)
                step.popupCanvas.SetActive(false);
        }

        Time.timeScale = 1f;
        foreach (var aud in audioSourcesToMute)
            aud.UnPause();

        tutorialRunning = false;
        Debug.Log("Tutorial completato!");
    }
}
