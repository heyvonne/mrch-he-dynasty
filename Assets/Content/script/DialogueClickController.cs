using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueClickController : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Audio")]
    public AudioSource audioSource;       // 在 Inspector 里拖入 AudioSource 组件

    [Header("Image")]
    public Image dialogueImage;            // 在 Inspector 里拖入 BlueFabricScrapPanel 里的 Image 组件
    public GameObject imagePanel;          // 在 Inspector 里拖入 BlueFabricScrapPanel 本身（用来控制显示/隐藏）

    private DialogueLine[] currentLines;
    private int currentIndex = 0;
    private bool isPlaying = false;

    /// <summary>
    /// 启动对话（接收带 Audio + Image 的 DialogueLine 数组）
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
    /// 兼容旧的 string[] 调用（项目中其他地方如果还在用 string[] 不会报错）
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

    /// <summary>
    /// 显示当前句子 + 播放对应音频 + 显示对应图片
    /// </summary>
    void ShowCurrentLine()
    {
        // ① 显示文本
        dialogueText.text = currentLines[currentIndex].text;

        // ② 播放音频（如果有）
        if (audioSource != null && currentLines[currentIndex].audio != null)
        {
            audioSource.PlayOneShot(currentLines[currentIndex].audio);
        }

        // ③ 显示图片（如果有）
        if (currentLines[currentIndex].image != null)
        {
            dialogueImage.sprite = currentLines[currentIndex].image;
            imagePanel.SetActive(true);
        }
        else
        {
            // 这句话没有配图 → 隐藏图片面板
            imagePanel.SetActive(false);
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

        // 对话结束时也隐藏图片面板
        if (imagePanel != null)
        {
            imagePanel.SetActive(false);
        }
    }

    public bool IsFinished()
    {
        return !isPlaying;
    }
}