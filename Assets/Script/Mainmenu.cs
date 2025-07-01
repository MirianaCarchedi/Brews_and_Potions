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
        SceneManager.LoadSceneAsync("Bancone_Laboratorio");
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
