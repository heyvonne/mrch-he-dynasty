using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string text;       // 对话文本
    public AudioClip audio;   // 对应的语音，可以为空
    public Sprite image;      // 对应的图片，可以为空（不需要图片的句子留空即可）

    public DialogueLine() { }

    public DialogueLine(string text, AudioClip audio = null, Sprite image = null)
    {
        this.text = text;
        this.audio = audio;
        this.image = image;
    }
}