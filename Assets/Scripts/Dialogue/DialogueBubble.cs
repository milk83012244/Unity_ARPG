using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using Febucci.UI.Core;

/// <summary>
/// ��ܮ�w��
/// </summary>
public class DialogueBubble : DialogueViewBase
{
    [SerializeField] RectTransform container; //��ܮ�
    [SerializeField] RectTransform characterContainer;//�����
    [SerializeField] TextMeshProUGUI CharacterNametext;
    public TypewriterCore typewriter;

    private string tempDialogueLine;

    Action advanceHandler;

    public TextMeshProUGUI dialogueLineText;


    private bool isTypewriterDone;
    /// <summary>
    /// �}�l���
    /// </summary>
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowBubble(true);
        //SetisInteractable(true);
        CharacterNametext.text = dialogueLine.CharacterName; //����W�٬�Yarn�}���W�� �o�ܪ�:
        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");     
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //�⨤��W�٧R����X��ܤ��e��r

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
    /// �������
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
    /// �ǤJ��{
    /// </summary>
    public void StartCoroutineWithCallback(IEnumerator coroutine,Action callback)
    {
        StartCoroutine(WaitForCoroutineToEnd(coroutine, callback));
    }
    /// <summary>
    /// �ʸ˨�{ �b��{�����ɦ^�ը��
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
    /// �]�w��ܮئ�m
    /// </summary>
    public void SetPosition(Transform target)
    {
        container.position = target.position;
    }

    /// <summary>
    /// ��U�@�y���
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
