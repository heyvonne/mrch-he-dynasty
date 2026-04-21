using UnityEngine;

public class SphereTrigger : MonoBehaviour
{
    public int index;
    public static int currentIndex = 0;

    [Header("Dialogue")]
    public string[] dialogueLines;
    public DialogueClickController dialogueController;

    [Header("Choice (only for sphere6)")]
    public GameObject choiceUI;

    [Header("NPC Sync")]
    public NPCMovement npc;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered && index == currentIndex)
        {
            hasTriggered = true;

            // 👉 通知NPC玩家到了
            if (npc != null)
            {
                npc.PlayerArrivedAtPoint(index);
            }

            if (dialogueLines != null && dialogueLines.Length > 0)
            {
                dialogueController.StartDialogue(dialogueLines);
                waitingForDialogue = true;
            }
            else
            {
                AfterDialogue();
            }
        }
    }

    void Update()
    {
        if (waitingForDialogue && dialogueController.IsFinished())
        {
            waitingForDialogue = false;
            AfterDialogue();
        }
    }

    void AfterDialogue()
    {
        // 👉 sphere6：显示按钮
        if (choiceUI != null)
        {
            choiceUI.SetActive(true);
        }
        else
        {
            Advance();
        }
    }

    public void Advance()
    {
        currentIndex++;
    }
}