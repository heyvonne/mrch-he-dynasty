using UnityEngine;
using System.Collections;

public class ChoiceManager : MonoBehaviour
{
    [Header("NPC & Trigger")]
    public NPCMovement npc;
    public SphereTrigger sphereTrigger;
    public Transform spawnPoint;

    [Header("Choice Panels")]
    public GameObject panelA;               // 选A时显示的图片 Panel
    public GameObject panelB;               // 选B时显示的图片 Panel
    public GameObject panelC;               // 选C时显示的图片 Panel
    public float panelDuration = 5f;        // 图片显示时长（秒），ABC共用

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

    // A/B：显示图片 → 等待 → 回到选项
    IEnumerator ShowPanelThenReturn(GameObject panel)
    {
        gameObject.SetActive(false);        // 隐藏选项面板

        if (panel != null) panel.SetActive(true);
        yield return new WaitForSeconds(panelDuration);
        if (panel != null) panel.SetActive(false);

        gameObject.SetActive(true);         // 重新显示选项面板
    }

    // C：显示图片 → 等待 → 继续剧情
    IEnumerator ShowPanelThenAdvance(GameObject panel)
    {
        gameObject.SetActive(false);        // 隐藏选项面板

        if (panel != null) panel.SetActive(true);
        yield return new WaitForSeconds(panelDuration);
        if (panel != null) panel.SetActive(false);

        // 继续后面剧情
        if (npc != null)
        {
            npc.transform.position = spawnPoint.position;
            npc.gameObject.SetActive(true);
            npc.StartMoving();
        }

        if (sphereTrigger != null)
        {
            sphereTrigger.Advance();
        }
    }
}