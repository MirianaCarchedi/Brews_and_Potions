using UnityEngine;

public class CameraMover : MonoBehaviour
{
    // Imposta la posizione target della camera
    public float targetX = 2000f;

    void Update()
    {
        // Quando premi il tasto "M", la camera si sposta
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3 newPosition = transform.position;
            newPosition.x = targetX;
            transform.position = newPosition;
        }
    }
}

