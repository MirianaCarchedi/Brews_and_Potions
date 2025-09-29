using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableSecond : MonoBehaviour
{

    public GameObject diciasette;

    public GameObject diciottesimo;


    public void OnClick2()
    {
        StartCoroutine(ClickWaitSecondPopUp(2));
    }
    public IEnumerator ClickWaitSecondPopUp(float delay)
    {
        diciasette.SetActive(true);
        yield return new WaitForSeconds(2f);
    }

    public void EnableDiciotto()
    {
        diciottesimo.SetActive(true);
    }


}
