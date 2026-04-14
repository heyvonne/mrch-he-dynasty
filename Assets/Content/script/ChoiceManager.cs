using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public NPCMovement npc;
    public GameObject wrongText;

    public void ChooseA()
    {
        ShowWrong();
    }

    public void ChooseB()
    {
        ShowWrong();
    }

    public void ChooseC()
    {
        // 正确答案
        if (npc != null)
        {
            npc.StartMovement();
        }

        gameObject.SetActive(false); // 关闭按钮
    }

    void ShowWrong()
    {
        if (wrongText != null)
        {
            wrongText.SetActive(true);
            Invoke(nameof(HideWrong), 4f);
        }
    }

    void HideWrong()
    {
        if (wrongText != null)
        {
            wrongText.SetActive(false);
        }
    }
}