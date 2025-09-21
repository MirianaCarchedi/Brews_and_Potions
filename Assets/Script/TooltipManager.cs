using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    [Header("UI Elements")]
    public GameObject tooltipPanel;
    public GameObject tooltipPanel2;
    public TextMeshProUGUI tooltipText;
    public TextMeshProUGUI tooltipText2;
    public Vector3 offset = new Vector3(0, 50, 0);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        tooltipPanel.SetActive(false);
        tooltipPanel2.SetActive(false);
    }

    private void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;

        if (tooltipPanel.activeSelf)
            tooltipPanel.transform.position = mousePos + offset;

        if (tooltipPanel2.activeSelf)
            tooltipPanel2.transform.position = mousePos + offset;
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        if (tooltipPanel.activeSelf)
            tooltipPanel.transform.position = mousePos + offset;

        if (tooltipPanel2.activeSelf)
            tooltipPanel2.transform.position = mousePos + offset;
    }

    public void Show(string message)
    {
        tooltipText.text = message;
        tooltipPanel.SetActive(true);

        tooltipText2.text = message;
        tooltipPanel2.SetActive(true);
    }

    public void Hide()
    {
        tooltipPanel.SetActive(false);
        tooltipPanel2.SetActive(false);
    }
}
