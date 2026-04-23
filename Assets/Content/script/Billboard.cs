using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform target;

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

        // ⭐ 只在水平旋转
        direction.y = 0;

        if (direction.sqrMagnitude < 0.001f) return;

        transform.rotation = Quaternion.LookRotation(direction);
    }
}