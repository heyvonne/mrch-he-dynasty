using UnityEngine;
using System.Collections;

public class NPCMovement : MonoBehaviour
{
    public Transform[] waypoints;   // 包含 Sphere + Mid点
    public float speed = 2f;

    private int currentTarget = 0;
    private bool canMove = false;

    public void StartMovement()
    {
        gameObject.SetActive(true);

        transform.position = waypoints[0].position;

        currentTarget = 1;

        StartCoroutine(MoveRoutine());
    }

    IEnumerator MoveRoutine()
    {
        while (currentTarget < waypoints.Length)
        {
            Transform target = waypoints[currentTarget];

            // ⭐ 移动（防穿墙 + 不贴近）
            while (Vector3.Distance(transform.position, target.position) > 1.2f)
            {
                Vector3 direction = (target.position - transform.position).normalized;

                float checkDistance = 0.8f;
                Ray ray = new Ray(transform.position + Vector3.up * 0.5f, direction);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, checkDistance))
                {
                    if (!hit.collider.CompareTag("Waypoint"))
                    {
                        yield return null;
                        continue;
                    }
                }

                transform.position += direction * speed * Time.deltaTime;

                yield return null;
            }

            // ⭐ 只有Sphere才触发
            SphereTrigger trigger = target.GetComponent<SphereTrigger>();

            if (trigger != null)
            {
                trigger.NPCArrived();

                // ⭐ 等待（只有Sphere需要等）
                canMove = false;
                yield return new WaitUntil(() => canMove);
            }

            // ⭐ Mid点不会停，直接走
            currentTarget++;
        }
    }

    public void AllowMove()
    {
        canMove = true;
    }
}