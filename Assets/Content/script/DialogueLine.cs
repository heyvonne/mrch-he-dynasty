using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string text;
    public AudioClip audio;    // 每句话对应的语音，可为空

    public DialogueLine() { }

    public DialogueLine(string text, AudioClip audio = null)
    {
        this.text = text;
        this.audio = audio;
    }
}
