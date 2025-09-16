using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [Header("UI Elements")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI tooltipText;
    public Vector3 offset = new Vector3(0, 50, 0);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        tooltipPanel.SetActive(false);
    }

    private void LateUpdate()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            tooltipPanel.transform.position = mousePos + offset;
        }
    }

    public void UpdatePosition()
    {
        if (tooltipPanel.activeSelf)
        {
            Vector3 mousePos = Input.mousePosition;
            tooltipPanel.transform.position = mousePos + offset;
        }
    }

    public void Show(string message)
    {
        tooltipText.text = message;
        tooltipPanel.SetActive(true);

    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
        
    }
}
