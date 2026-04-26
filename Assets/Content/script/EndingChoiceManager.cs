using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingChoiceManager : MonoBehaviour
{
    [Header("Ending Panels")]
    public GameObject endingPanelA;         // 结局A的 Panel（拖入对应的 GameObject）
    public Sprite endingImageA;             // 结局A的图片（从 Project 窗口拖入 Sprite）
    [TextArea] public string endingTextA;   // 结局A的文字

    public GameObject endingPanelB;         // 结局B的 Panel
    public Sprite endingImageB;             // 结局B的图片
    [TextArea] public string endingTextB;   // 结局B的文字

    [Header("References")]
    public Image endingImageDisplay;        // 通用结局图片展示区（跟 DialogueClickController 里的 ImagePanel 类似）
    public TextMeshProUGUI endingTextDisplay; // 结局文字展示区
    public GameObject endingPanel;          // 包含上面两者的父 Panel

    [Header("Settings")]
    public float typewriterSpeed = 0.05f;   // 打字机效果的每个字间隔（秒）

    private Coroutine _typewriterCoroutine;

    public void ChooseEndingA()
    {
        ShowEnding(endingImageA, endingTextA);
    }

    public void ChooseEndingB()
    {
        ShowEnding(endingImageB, endingTextB);
    }

    void ShowEnding(Sprite image, string text)
    {
        // 隐藏选择面板
        gameObject.SetActive(false);

        // 隐藏 GuidePanel 和其他 UI（可选）
        // 如果你有 GuidePanel 的引用可以在这里隐藏

        // 显示结局 Panel
        endingPanel.SetActive(true);

        // 显示结局图片
        if (endingImageDisplay != null && image != null)
        {
            endingImageDisplay.sprite = image;
        }

        // 打字机效果显示结局文字
        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);
        _typewriterCoroutine = StartCoroutine(TypewriterEffect(text));
    }

    IEnumerator TypewriterEffect(string fullText)
    {
        endingTextDisplay.text = "";

        foreach (char c in fullText)
        {
            endingTextDisplay.text += c;
            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
}
