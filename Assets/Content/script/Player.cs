using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // 左右方向键
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }

        // 上下方向键
        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveZ = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveZ = -1f;
        }

        Vector3 move = new Vector3(moveX, 0, moveZ);

        transform.Translate(move * speed * Time.deltaTime, Space.World);
    }
}