using System.Collections;
using UnityEngine;
using TMPro;

public class TypingFinal : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float delay = 0.05f;
    public string initialText = ""; 

    private void Start()
    {
        StartTyping(initialText);
    }

    public void StartTyping(string fullText)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string fullText)
    {
        text.text = "";
        foreach (char c in fullText)
        {
            text.text += c;
            yield return new WaitForSeconds(delay);
        }
    }
}

