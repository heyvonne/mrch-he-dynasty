using UnityEngine;

public class ChoiceManager : MonoBehaviour
{
    public NPCMovement npc;
    public GameObject wrongText;

    public SphereTrigger sphereTrigger;
    public Transform spawnPoint; // sphere6

    private bool hasChosenCorrect = false;

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
        if (hasChosenCorrect) return;
        hasChosenCorrect = true;

        if (npc != null)
        {
            npc.transform.position = spawnPoint.position; // ⭐ 在6生成
            npc.gameObject.SetActive(true);
            npc.StartMoving();
        }

        if (sphereTrigger != null)
        {
            sphereTrigger.Advance();
        }

        gameObject.SetActive(false);
    }

    void ShowWrong()
    {
        if (wrongText != null)
        {
            wrongText.SetActive(true);
            CancelInvoke(nameof(HideWrong));
            Invoke(nameof(HideWrong), 2f);
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