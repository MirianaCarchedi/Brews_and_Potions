using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    public static GameInputManager Instance;
    private bool inputEnabled = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    public bool IsInputEnabled()
    {
        return inputEnabled;
    }
}
