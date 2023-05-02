using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using static Yarn.Unity.Effects;
using System.Threading.Tasks;

/// <summary>
/// ��ܮ�w��
/// </summary>
public class DialogueBubble : DialogueViewBase
{
    [SerializeField] RectTransform container; //��ܮ�
    [SerializeField] RectTransform characterContainer;//�����
    [SerializeField] TextMeshProUGUI CharacterNametext;

    private string tempDialogueLine;

    Action advanceHandler;

    public TextMeshProUGUI dialogueLineText;

    public float typeSpeed = 10f; //���r�t��(�@��)

    private bool isTypewriterDone;
    /// <summary>
    /// �}�l���
    /// </summary>
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        isTypewriterDone = false;

        ShowBubble(true);
        SetisInteractable(true);
        CharacterNametext.text = dialogueLine.CharacterName; //����W�٬�Yarn�}���W�� �o�ܪ�:
        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");     
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //�⨤��W�٧R����X��ܤ��e��r

        //StartCoroutine(Typewriter(dialogueLineText, typeSpeed,null));
        StartTypewriterCoroutine();

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

        SetisInteractable(false);
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
    /// <summary>
    /// ������w��{ �����ɦ^��Action���e
    /// </summary>
    public void StartTypewriterCoroutine()
    {
        StartCoroutineWithCallback(Typewriter(dialogueLineText, typeSpeed, null), () =>
         {
             isTypewriterDone = true;
         });
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

    public void SetisInteractable(bool isInteractable)
    {
        GameManager.GetInstance().isInteractable = isInteractable;
    }
    /// <summary>
    /// ��U�@�y���
    /// </summary>
    public override void UserRequestedViewAdvancement()
    {
        if (container.gameObject.activeSelf)
        {
            if (isTypewriterDone)
            {
                requestInterrupt?.Invoke();
            }
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
