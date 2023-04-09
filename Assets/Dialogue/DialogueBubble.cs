using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using TMPro;
using static Yarn.Unity.Effects;

/// <summary>
/// ��ܮ�w��
/// </summary>
public class DialogueBubble : DialogueViewBase
{
    [SerializeField] RectTransform container; //��ܮ�
    [SerializeField] RectTransform characterContainer;//�����
    [SerializeField] TextMeshProUGUI CharacterNametext;

    private string tempDialogueLine;
    private CoroutineInterruptToken typewriterToken;

    Action advanceHandler;

    public TextMeshProUGUI dialogueLineText;

    public float typeSpeed = 10f; //���r�t��(�@��)

    /// <summary>
    /// �}�l���
    /// </summary>
    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        ShowBubble(true);
        SetisInteractable(true);
        CharacterNametext.text = dialogueLine.CharacterName; //����W�٬�Yarn�}���W�� �o�ܪ�:
        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");     
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //�⨤��W�٧R����X��ܤ��e��r

        StartCoroutine(Typewriter(dialogueLineText, 10f, null));

        advanceHandler = requestInterrupt;
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
