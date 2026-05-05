using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingChoiceManager : MonoBehaviour
{
    [Header("Ending Panels")]
    public GameObject endingPanelA;
    public Sprite endingImageA;
    [TextArea] public string endingTextA;

    public GameObject endingPanelB;
    public Sprite endingImageB;
    [TextArea] public string endingTextB;

    [Header("References")]
    public Image endingImageDisplay;
    public TextMeshProUGUI endingTextDisplay;
    public GameObject endingPanel;

    [Header("Settings")]
    public float typewriterSpeed = 0.05f;

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
        endingPanel.SetActive(true);

        if (endingImageDisplay != null && image != null)
        {
            endingImageDisplay.sprite = image;
        }

        if (_typewriterCoroutine != null)
            StopCoroutine(_typewriterCoroutine);
        _typewriterCoroutine = StartCoroutine(TypewriterEffect(text));

        gameObject.SetActive(false);
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