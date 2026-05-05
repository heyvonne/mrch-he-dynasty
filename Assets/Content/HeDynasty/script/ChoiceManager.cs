using UnityEngine;
using System.Collections;

public class ChoiceManager : MonoBehaviour
{
    [Header("NPC & Trigger")]
    public NPCMovement npc;
    public SphereTrigger sphereTrigger;
    public Transform spawnPoint;

    [Header("Choice UI Root（三个按钮的父节点，拖入这里）")]
    public GameObject choiceUIRoot;           // ★ 关键：只关闭这个，不关闭ChoiceManager自身

    [Header("Choice Panels（ABC各自对应的图片Panel）")]
    public GameObject panelA;
    public GameObject panelB;
    public GameObject panelC;

    public GameObject triger7Explain1;
    public GameObject triger8Explain2;
    public GameObject triger9Ending;

    private bool hasChosenCorrect = false;
    private Coroutine _currentCoroutine;

    public void ChooseA()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ShowPanelThenReturn(panelA));
    }

    public void ChooseB()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ShowPanelThenReturn(panelB));
    }

    public void ChooseC()
    {
        if (hasChosenCorrect) return;
        hasChosenCorrect = true;

        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        _currentCoroutine = StartCoroutine(ShowPanelThenAdvance(panelC));
    }

    // A/B：隐藏选项按钮 → 显示图片 → 等待点击 → 隐藏图片 → 重新显示选项按钮
    IEnumerator ShowPanelThenReturn(GameObject panel)
    {
        // ★ 只隐藏按钮UI，不关闭ChoiceManager自身（协程才能继续运行）
        if (choiceUIRoot != null) choiceUIRoot.SetActive(false);

        if (panel != null) panel.SetActive(true);

        Debug.Log("【ChoiceManager】显示Panel，等待点击关闭...");

        yield return WaitForClick();

        Debug.Log("【ChoiceManager】检测到点击，关闭Panel");

        if (panel != null) panel.SetActive(false);

        // ★ 重新显示选项按钮，回到选择
        if (choiceUIRoot != null) choiceUIRoot.SetActive(true);
    }

    // C：隐藏选项按钮 → 显示图片 → 等待点击 → 隐藏图片 → 继续剧情
    IEnumerator ShowPanelThenAdvance(GameObject panel)
    {
        if (choiceUIRoot != null) choiceUIRoot.SetActive(false);

        if (panel != null) panel.SetActive(true);

        Debug.Log("【ChoiceManager】选项C，等待点击继续剧情...");

        yield return WaitForClick();

        Debug.Log("【ChoiceManager】检测到点击，继续剧情");

        if (panel != null) panel.SetActive(false);

        // 继续后面剧情
        if (npc != null)
        {
            npc.transform.position = spawnPoint.position;
            npc.gameObject.SetActive(true);
            npc.StartMoving();
            
            // active triger7Explain1
            triger7Explain1.SetActive(true);
            triger8Explain2.SetActive(true);
            triger9Ending.SetActive(true);
        }

        if (sphereTrigger != null)
        {
            sphereTrigger.Advance();
        }
    }

    // 等待玩家点击鼠标左键
    IEnumerator WaitForClick()
    {
        // 先等一帧，防止刚点了选项按钮就立刻触发这里的点击
        yield return null;

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
    }
}