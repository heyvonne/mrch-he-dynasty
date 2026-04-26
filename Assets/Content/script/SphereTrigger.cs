using UnityEngine;
using System.Collections;

public class SphereTrigger : MonoBehaviour
{
    public int index;
    public static int currentIndex = 0;

    [Header("Dialogue")]
    public DialogueLine[] dialogueLines;
    public DialogueClickController dialogueController;

    [Header("Choice (only for sphere6)")]
    public GameObject choiceUI;

    [Header("NPC Sync")]
    public NPCMovement npc;

    [Header("Popup Panel (auto show & hide)")]
    public GameObject popupPanel;          // ← 拖入 ShenbaoPanel
    public float popupDuration = 10f;      // ← 显示时长（秒）

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
            // ⭐ 扩展事件
            // =========================
            if (events != null && events.onEnter != null)
            {
                events.onEnter.Invoke();
            }

            // =========================
            // ④ Popup Panel（新增）
            // =========================
            if (popupPanel != null)
            {
                StartCoroutine(ShowPopupThenHide());
            }

            // =========================
            // ② Dialogue逻辑
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

    // =========================
    // Popup Panel 逻辑（新增）
    // =========================
    IEnumerator ShowPopupThenHide()
    {
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        popupPanel.SetActive(false);
    }
}