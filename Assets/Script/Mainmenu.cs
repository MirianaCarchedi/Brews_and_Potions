using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    private const string saveKey = "GameSaved";  // flag semplice per sapere se c'è salvataggio

    public void NewGame()
    {
        PlayerPrefs.DeleteKey(saveKey);  // resetta il salvataggio
        PlayerPrefs.SetInt(saveKey, 0);
        PlayerPrefs.Save();

        Time.timeScale = 1f;
        SceneManager.LoadScene("Bancone_Laboratorio");
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            int val = PlayerPrefs.GetInt(saveKey);
            Debug.Log($"SaveKey trovato con valore: {val}");
            if (val == 1)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Bancone_Laboratorio");
                return;
            }
        }
        Debug.Log("Nessun salvataggio trovato!");
    }



    public void ReturnToMainMenu()
    {
        SaveGame();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
        Debug.Log("Bye Bye");
    }

    public void EndGame()
    {
        PlayerPrefs.DeleteKey(saveKey);  // resetta il salvataggio
        Application.Quit();
        Debug.Log("Bye Bye");
    }

    private void SaveGame()
    {
        // Qui metti tutto quello che vuoi salvare, per ora solo il flag semplice
        PlayerPrefs.SetInt(saveKey, 1);
        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }

}