using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;

    private int currentIndex = 0;
    private bool isMoving = false;

    // 跟踪NPC和玩家是否都到达当前交互点
    private bool npcArrived = false;
    private bool playerArrived = false;

    // 哪些点需要等待交互（填对应的 index，比如 sphere7/8/9）
    public int[] stopPointIndices;

    // 对话结束的信号
    private bool dialogueFinished = false;

    public void StartMoving()
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine());
        }
    }

    IEnumerator MoveRoutine()
    {
        isMoving = true;

        while (currentIndex < waypoints.Length)
        {
            Transform target = waypoints[currentIndex];

            // NPC 移动到目标点
            while (Vector3.Distance(transform.position, target.position) > 0.2f)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;
                yield return null;
            }

            // NPC 到达当前点
            npcArrived = true;

            // 如果是关键交互点，等玩家也到了 + 对话结束
            if (IsStopPoint(currentIndex))
            {
                // ① 等玩家到达（由 SphereTrigger 调用 PlayerArrivedAtPoint）
                yield return new WaitUntil(() => playerArrived);

                // ② 等对话结束（由 SphereTrigger 调用 OnDialogueFinished）
                yield return new WaitUntil(() => dialogueFinished);

                // ③ 对话结束后等1秒
                yield return new WaitForSeconds(1f);

                // 重置状态，准备下一个点
                playerArrived = false;
                dialogueFinished = false;
            }

            npcArrived = false;
            currentIndex++;
        }

        isMoving = false;
    }

    bool IsStopPoint(int index)
    {
        foreach (int i in stopPointIndices)
        {
            if (i == index) return true;
        }
        return false;
    }

    // ====== 外部调用接口 ======

    // SphereTrigger 在玩家进入时调用
    public void PlayerArrivedAtPoint()
    {
        playerArrived = true;
    }

    // SphereTrigger 在对话结束时调用
    public void OnDialogueFinished()
    {
        dialogueFinished = true;
    }
}
