using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;
    public bool lockY = true;
    public float yOffset = 180f; // ⭐ 用来修正正反

    void Start()
    {
        if (target == null && Camera.main != null)
        {
            target = Camera.main.transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        if (lockY)
        {
            direction.y = 0;
        }

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion lookRot = Quaternion.LookRotation(direction);

        // ⭐ 关键修复：翻转180度解决“背对玩家”
        transform.rotation = lookRot * Quaternion.Euler(0, yOffset, 0);
    }
}