using UnityEngine;
using TMPro;

public class DialogueClickController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Audio")]
    public AudioSource audioSource;   // 拖入同一个物体上的 AudioSource 组件

    private DialogueLine[] currentLines;
    private int currentIndex = 0;
    private bool isPlaying = false;

    /// <summary>
    /// 接收带 Audio 的对话行
    /// </summary>
    public void StartDialogue(DialogueLine[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        currentLines = lines;
        currentIndex = 0;
        isPlaying = true;

        dialoguePanel.SetActive(true);
        ShowCurrentLine();
    }

    /// <summary>
    /// 兼容旧的 string[] 调用方式（保留向后兼容）
    /// </summary>
    public void StartDialogue(string[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        var converted = new DialogueLine[lines.Length];
        for (int i = 0; i < lines.Length; i++)
            converted[i] = new DialogueLine(lines[i]);

        StartDialogue(converted);
    }

    void Update()
    {
        if (isPlaying && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }

    void ShowCurrentLine()
    {
        dialogueText.text = currentLines[currentIndex].text;

        // ⭐ 同步播放该句对应的音频
        if (audioSource != null && currentLines[currentIndex].audio != null)
        {
            audioSource.PlayOneShot(currentLines[currentIndex].audio);
        }
    }

    void NextLine()
    {
        currentIndex++;

        if (currentIndex < currentLines.Length)
        {
            ShowCurrentLine();
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

