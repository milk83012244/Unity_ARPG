using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using Febucci.UI.Core;

/// <summary>
/// 對話氣泡框
/// </summary>
public class DialogueBubble : DialogueViewBase
{
    [SerializeField] RectTransform container; //對話框
    [SerializeField] RectTransform characterContainer;//角色框
    [SerializeField] TextMeshProUGUI CharacterNametext;
    public TypewriterCore typewriter;

    private string tempDialogueLine;

    Action advanceHandler;

    public TextMeshProUGUI dialogueLineText;


    private bool isTypewriterDone;
    /// <summary>
    /// 開始對話
    /// </summary>
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowBubble(true);
        //SetisInteractable(true);
        CharacterNametext.text = dialogueLine.CharacterName; //角色名稱為Yarn腳本上的 發話者:
        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");     
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //把角色名稱刪除輸出對話內容文字

        //StartCoroutine(Typewriter(dialogueLineText, typeSpeed,null));
        typewriter.ShowText(dialogueLineText.text);
        //StartTypewriterCoroutine();

        //advanceHandler = requestInterrupt;
    }
    public override void InterruptLine(LocalizedLine dialogueLine, Action onInterruptLineFinished)
    {
        onInterruptLineFinished?.Invoke();
    }
    /// <summary>
    /// 結束對話
    /// </summary>
    public override void DismissLine(Action onDismissalComplete)
    {
        CharacterNametext.text = "";
        dialogueLineText.text = "";
        StopAllCoroutines();
        ShowBubble(false);

        //SetisInteractable(false);
        onDismissalComplete();
    }
    /// <summary>
    /// 傳入協程
    /// </summary>
    public void StartCoroutineWithCallback(IEnumerator coroutine,Action callback)
    {
        StartCoroutine(WaitForCoroutineToEnd(coroutine, callback));
    }
    /// <summary>
    /// 封裝協程 在協程完成時回調函數
    /// </summary>
    private IEnumerator WaitForCoroutineToEnd(IEnumerator coroutine, Action callback)
    {
        yield return StartCoroutine(coroutine);
        callback();
    }

    public void ShowBubble(bool show)
    {
        container.gameObject.SetActive(show);
    }
    /// <summary>
    /// 設定對話框位置
    /// </summary>
    public void SetPosition(Transform target)
    {
        container.position = target.position;
    }

    /// <summary>
    /// 到下一句對話
    /// </summary>
    public override void UserRequestedViewAdvancement()
    {
        if (container.gameObject.activeSelf)
        {
            //if (isTypewriterDone)
            //{
            //    requestInterrupt?.Invoke();
            //}
            requestInterrupt?.Invoke();
        }
    }
    private void Update()
    {
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dialogueLineText.preferredWidth + 11f);
        characterContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CharacterNametext.preferredWidth + 5f);
        if (Input.GetKeyDown(KeyCode.P))
        {
            UserRequestedViewAdvancement();
        }
    }
}
