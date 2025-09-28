using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TutorialStep
{
    public string stepName;           // Nome descrittivo dello step
    public string popupText;          // Testo del popup
    public GameObject popupCanvas;    // Il Canvas da attivare
    public Button nextButton;         // Pulsante "Next" per passare allo step successivo
    public float autoHideTime = 0f;  // Tempo dopo il quale il popup si nasconde automaticamente (0 = non auto-hide)
}
