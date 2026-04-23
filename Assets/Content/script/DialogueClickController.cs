using UnityEngine;
using TMPro;

public class DialogueClickController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    private string[] currentLines;
    private int currentIndex = 0;
    private bool isPlaying = false;

    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentIndex = 0;
        isPlaying = true;

        dialoguePanel.SetActive(true);
        dialogueText.text = currentLines[currentIndex];
    }

    void Update()
    {
        if (isPlaying && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex < currentLines.Length)
        {
            dialogueText.text = currentLines[currentIndex];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isPlaying = false;
        dialoguePanel.SetActive(false);
    }

    public bool IsFinished()
    {
        return !isPlaying;
    }
}
