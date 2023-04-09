using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using static Yarn.Unity.Effects;

/// <summary>
/// 對話氣泡框
/// </summary>
public class DialogueBubble : DialogueViewBase
{
    [SerializeField] RectTransform container; //對話框
    [SerializeField] RectTransform characterContainer;//角色框
    [SerializeField] TextMeshProUGUI CharacterNametext;

    private string tempDialogueLine;
    private CoroutineInterruptToken typewriterToken;

    Action advanceHandler;

    public TextMeshProUGUI dialogueLineText;

    public float typeSpeed = 10f; //打字速度(毫秒)

    /// <summary>
    /// 開始對話
    /// </summary>
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowBubble(true);
        SetisInteractable(true);
        CharacterNametext.text = dialogueLine.CharacterName; //角色名稱為Yarn腳本上的 發話者:
        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");     
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //把角色名稱刪除輸出對話內容文字

        StartCoroutine(Typewriter(dialogueLineText, 10f, null));

        advanceHandler = requestInterrupt;
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

        SetisInteractable(false);
        onDismissalComplete();
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

    public void SetisInteractable(bool isInteractable)
    {
        GameManager.GetInstance().isInteractable = isInteractable;
    }

    /// <summary>
    /// 到下一句對話
    /// </summary>
    public override void UserRequestedViewAdvancement()
    {
        if (container.gameObject.activeSelf)
        {
            advanceHandler?.Invoke();
        }
    }
    private void Update()
    {
        container.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dialogueLineText.preferredWidth + 11f);
        characterContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CharacterNametext.preferredWidth + 11f);
        if (Input.GetKeyDown(KeyCode.P))
        {
            UserRequestedViewAdvancement();
        }
    }
}
