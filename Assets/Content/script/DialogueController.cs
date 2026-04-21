using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public void PlayDialogue(string[] lines, float duration)
    {
        if (dialogueText == null)
        {
            Debug.LogError("DialogueText 没有连接！");
            return;
        }

        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(ShowLines(lines, duration));
    }

    IEnumerator ShowLines(string[] lines, float duration)
    {
        foreach (string line in lines)
        {
            dialogueText.text = line;
            yield return new WaitForSeconds(duration);
        }

        gameObject.SetActive(false);
    }
}