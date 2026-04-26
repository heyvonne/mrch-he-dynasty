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

    [Header("NEW: Extendable Events (IMPORTANT)")]
    public SphereTriggerEvents events;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            !hasTriggered &&
            index == currentIndex)
        {
            hasTriggered = true;

            // =========================
            // ① NPC逻辑（不改）
            // =========================
            if (npc != null)
            {
                npc.PlayerArrivedAtPoint(index);
            }

            // =========================
            // ⭐ NEW：扩展事件（核心）
            // =========================
            if (events != null && events.onEnter != null)
            {
                events.onEnter.Invoke();
            }

            // =========================
            // ② Dialogue逻辑（不改）
            // =========================
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
        // =========================
        // ③ Choice逻辑（不改）
        // =========================
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