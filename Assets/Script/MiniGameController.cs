using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGameController : MonoBehaviour
{
    public RectTransform draggableImage;
    public RectTransform targetArea;
    public float successThreshold = 50f;
    private Action onSuccess;
    private bool isPlaying = false;

    public void StartMiniGame(Action onComplete)
    {
        onSuccess = onComplete;
        isPlaying = true;
        gameObject.SetActive(true); // attiva il canvas o pannello del minigioco
    }

    public void EndMiniGame()
    {
        isPlaying = false;
        gameObject.SetActive(false);
        onSuccess?.Invoke();
    }

    private void Update()
    {
        if (!isPlaying) return;

        // Input drag semplice orizzontale
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                draggableImage.parent as RectTransform,
                Input.mousePosition,
                null,
                out mousePos
            );

            Vector3 newPos = draggableImage.localPosition;
            newPos.x = mousePos.x; // Solo asse X
            draggableImage.localPosition = newPos;
        }

        // Controlla se è nella zona target
        float distance = Mathf.Abs(draggableImage.localPosition.x - targetArea.localPosition.x);
        if (distance <= successThreshold)
        {
            EndMiniGame();
        }
    }
}
