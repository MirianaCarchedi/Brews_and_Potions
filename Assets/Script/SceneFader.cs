using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneFader : MonoBehaviour
{
    public Animator fadeAnimator;
    public string sceneToLoad;
    public Image sfondoNero;
    

    public void FadeToScene(string sceneName)
    {
        sfondoNero.gameObject.SetActive(true);
        sceneToLoad = sceneName;
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {        
        fadeAnimator.SetTrigger("FadeIn_Animation");
        
        yield return new WaitForSeconds(2f); 
        SceneManager.LoadScene(sceneToLoad);
    }

}
