using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pannellodiciasette : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Time.timeScale = 0f;
    }
    
    public void PlayTextTime()
    {
        Time.timeScale = 1f;
    }

}
