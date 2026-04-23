using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float waitTime = 4f;

    private int currentIndex = 0;
    private bool isMoving = false;

    private bool npcArrived = false;
    private bool playerArrived = false;

    // 👉 哪些点需要停（填 sphere7/8/9 的 index）
    public int[] stopPointIndices;

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

            // 移动
            while (Vector3.Distance(transform.position, target.position) > 0.2f)
            {
                Vector3 dir = (target.position - transform.position).normalized;
                transform.position += dir * speed * Time.deltaTime;
                yield return null;
            }

            npcArrived = true;

            // 👉 如果是关键点（7/8/9）
            if (IsStopPoint(currentIndex))
            {
                // 等玩家也到
                yield return new WaitUntil(() => playerArrived);

                // 等对话结束
                yield return new WaitForSeconds(waitTime);

                playerArrived = false;
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

    // 👉 Sphere调用
    public void PlayerArrivedAtPoint(int sphereIndex)
    {
        playerArrived = true;
    }
}