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

    [Header("Ending Choice (only for final sphere)")]
    public GameObject endingChoiceUI;       // ← 拖入结局选择 Panel

    [Header("NPC Sync")]
    public NPCMovement npc;

    [Header("Popup Panel (auto show & hide)")]
    public GameObject popupPanel;
    public float popupDuration = 10f;

    [Header("Guide Panel")]
    public GameObject guidePanel;
    public GameObject guideText;

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
            // ④ Popup Panel
            // =========================
            if (popupPanel != null)
            {
                StartCoroutine(ShowPopupThenHide());
            }

            // =========================
            // ⑤ Guide Panel
            // =========================
            if (guidePanel != null)
            {
                guidePanel.SetActive(true);
            }
            if (guideText != null)
            {
                guideText.SetActive(true);
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
        // ③ 中间选项逻辑（sphere6 的 ChoiceManager）
        // =========================
        if (choiceUI != null)
        {
            choiceUI.SetActive(true);
        }
        // =========================
        // ⑥ 结局选项逻辑（最后一个 sphere 的 EndingChoiceManager）
        // =========================
        else if (endingChoiceUI != null)
        {
            endingChoiceUI.SetActive(true);
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

    IEnumerator ShowPopupThenHide()
    {
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        popupPanel.SetActive(false);
    }
}