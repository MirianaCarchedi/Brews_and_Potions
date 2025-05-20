using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float delay = 0.05f;

    // Questo è il nuovo metodo pubblico
    public void StartTyping(string fullText)
    {
        StopAllCoroutines(); // utile se vuoi riavviare
        StartCoroutine(TypeText(fullText));
    }

    // Rimane privato, non serve accedervi da fuori
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
