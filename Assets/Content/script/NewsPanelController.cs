using UnityEngine;

public class NewsPanelController : MonoBehaviour
{
    public GameObject panel;

    // ⭐必须是 public + void + 无参数
    public void Show()
    {
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }

    void Update()
    {
        if (panel.activeSelf && Input.GetMouseButtonDown(0))
        {
            Hide();
        }
    }
}