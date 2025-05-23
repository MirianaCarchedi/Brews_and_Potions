using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour
{
    public void PlayGAme()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Prova_Bancone");
    }

    public void QuitGAme()
    {
        Application.Quit();
        Debug.Log("Bye Bye");
    }

    public void OpenLevel()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
