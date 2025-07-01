using UnityEngine;
using UnityEngine.UI;
using System;

public class MiniGameController : MonoBehaviour
{
    public RectTransform draggableImage;
    public float successThreshold = 200f;
    public float minX = -200f;
    public float maxX = 200f;

    private Vector3 startPosition;
    private Action onSuccess;
    public bool isPlaying = false;
    public bool IsPlaying => isPlaying;



    public void StartMiniGame(Action onComplete)
    {
        onSuccess = onComplete;
        isPlaying = true;

        startPosition = draggableImage.localPosition;
        gameObject.SetActive(true);

        // Abilita il drag
        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (dragScript != null)
            dragScript.enabled = true;
    }



    public void EndMiniGame()
    {
        if (!isPlaying) return;

        isPlaying = false;
        gameObject.SetActive(false);

        draggableImage.localPosition = startPosition;

        var dragScript = draggableImage.GetComponent<DragToSide>();
        if (dragScript != null)
            dragScript.enabled = false;

        onSuccess?.Invoke();
        onSuccess = null;
    }




    private void Update()
    {
        if (!isPlaying) return;

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
            newPos.x = Mathf.Clamp(mousePos.x, minX, maxX); // Solo movimento su X
            newPos.y = startPosition.y; // Fissa la Y
            draggableImage.localPosition = newPos;
        }

        float distanceMoved = Mathf.Abs(draggableImage.localPosition.x - startPosition.x);
        if (distanceMoved >= successThreshold)
        {
            EndMiniGame();
        }
    }
}
