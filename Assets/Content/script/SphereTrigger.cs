using UnityEngine;

public class SphereTrigger : MonoBehaviour
{
    public int index;
    public static int currentIndex = 0;

    public GameObject textObject;
    public NPCMovement npc;
    public GameObject buttonPanel; // 只有 Sphere6 用

    private bool triggered = false;

    private bool playerInside = false;
    private bool npcArrived = false;

    void Start()
    {
        if (textObject != null)
            textObject.SetActive(false);

        if (buttonPanel != null)
            buttonPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (!other.CompareTag("Player")) return;

        playerInside = true;

        // ❗ 必须按顺序
        if (index != currentIndex) return;

        // 🟢 Sphere6（index=5）
        if (index == 5)
        {
            ShowText();

            if (buttonPanel != null)
                buttonPanel.SetActive(true);

            triggered = true;
            currentIndex++;
            return;
        }

        // 🟡 Sphere7-9（需要NPC）
        if (index >= 6 && index <= 8)
        {
            TryShowText();
            return;
        }

        // 🔵 Sphere1-5
        ShowText();

        triggered = true;
        currentIndex++;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    // NPC 到达调用
    public void NPCArrived()
    {
        npcArrived = true;
        TryShowText();
    }

    void TryShowText()
    {
        if (!triggered && playerInside && npcArrived && index == currentIndex)
        {
            ShowText();

            triggered = true;
            currentIndex++;
        }
    }

    void ShowText()
    {
        if (textObject != null)
        {
            textObject.SetActive(true);
            Invoke(nameof(HideText), 4f);
        }

        // 只给 NPC 用（6之后才有意义）
        Invoke(nameof(AllowNPCToMove), 4f);
    }

    void AllowNPCToMove()
    {
        if (npc != null)
            npc.AllowMove();
    }

    void HideText()
    {
        if (textObject != null)
            textObject.SetActive(false);
    }
}