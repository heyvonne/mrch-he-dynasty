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
    public GameObject endingChoiceUI;

    [Header("NPC Sync")]
    public NPCMovement npc;

    [Header("Popup Panel (auto show & hide)")]
    public GameObject popupPanel;
    public float popupDuration = 10f;

    [Header("Guide Panel")]
    public GameObject guidePanel;
    public GameObject guideText;

    private bool hasTriggered = false;
    private bool waitingForDialogue = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") &&
            !hasTriggered &&
            index == currentIndex)
        {
            hasTriggered = true;

            // ① 通知NPC：玩家到达了
            if (npc != null)
            {
                npc.PlayerArrivedAtPoint();
            }

            // ② Popup Panel
            if (popupPanel != null)
            {
                StartCoroutine(ShowPopupThenHide());
            }

            // ③ Guide Panel
            if (guidePanel != null)
            {
                guidePanel.SetActive(true);
            }
            if (guideText != null)
            {
                guideText.SetActive(true);
            }

            // ④ Dialogue
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
        // ⑤ 中间选项逻辑（sphere6）
        if (choiceUI != null)
        {
            choiceUI.SetActive(true);
        }
        // ⑥ 结局选项逻辑（最后一个sphere）
        else if (endingChoiceUI != null)
        {
            endingChoiceUI.SetActive(true);
        }
        else
        {
            // 普通sphere：对话结束 → 通知NPC → 前进
            Advance();
        }
    }

    public void Advance()
    {
        // 通知NPC：对话（以及可能的选项）全部结束了
        if (npc != null)
        {
            npc.OnDialogueFinished();
        }

        currentIndex++;
    }

    IEnumerator ShowPopupThenHide()
    {
        popupPanel.SetActive(true);
        yield return new WaitForSeconds(popupDuration);
        popupPanel.SetActive(false);
    }
}
