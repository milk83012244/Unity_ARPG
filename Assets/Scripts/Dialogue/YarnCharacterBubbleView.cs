using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using Febucci.UI.Core;

public class YarnCharacterBubbleView : DialogueViewBase
{
    public static YarnCharacterBubbleView instance;

    public List<YarnCharacter> allCharacters = new List<YarnCharacter>(); //����M��

    public YarnCharacter playerCharacter;
    private YarnCharacter speakerCharacter;

    private string tempDialogueLine;

    [SerializeField] TextMeshProUGUI CharacterNametext;
    public TextMeshProUGUI dialogueLineText;

    public TypewriterCore typewriter;

    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public RectTransform dialogueBubbleRect, optionsBubbleRect,characterNameRect;
    public float bubbleMargin = 0.1f;

    private Camera worldCamera;

    private void Awake()
    {
        instance = this;
        worldCamera = Camera.main;
    }
    public void RegisterYarnCharacter(YarnCharacter newCharacter)
    {
        if (!YarnCharacterBubbleView.instance.allCharacters.Contains(newCharacter))
        {
            allCharacters.Add(newCharacter);
        }
    }
    public void ForgetYarnCharacter(YarnCharacter deletedCharacter)
    {
        if (YarnCharacterBubbleView.instance.allCharacters.Contains(deletedCharacter))
        {
            allCharacters.Remove(deletedCharacter);
        }
    }
    YarnCharacter FindCharacter(string searchName)
    {
        foreach (var character in allCharacters)
        {
            if (character.characterName == searchName)
            {
                return character;
            }
        }

        Debug.LogWarningFormat("YarnCharacterBubbleView�䤣�쨤��W�� {0}!", searchName);
        return null;
    }
    /// <summary>
    ///  �p���ܮئ�m�O���b�ù���
    /// </summary>
    Vector2 WorldToAnchoredPosition(RectTransform bubble, Vector3 worldPos, float constrainToViewportMargin = -1f)
    {
        Camera canvasCamera = worldCamera;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            canvasCamera = null;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            bubble.parent.GetComponent<RectTransform>(),
            worldCamera.WorldToScreenPoint(worldPos),
            canvasCamera,
            out Vector2 screenPos
        );

        if (constrainToViewportMargin >= 0f)
        {
            bool useCanvasResolution = canvasScaler != null && canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ConstantPixelSize;
            Vector2 screenSize = Vector2.zero;
            screenSize.x = useCanvasResolution ? canvasScaler.referenceResolution.x : Screen.width;
            screenSize.y = useCanvasResolution ? canvasScaler.referenceResolution.y : Screen.height;

            var halfBubbleWidth = bubble.rect.width / 2;
            var halfBubbleHeight = bubble.rect.height / 2;

            var margin = screenSize.x < screenSize.y ? screenSize.x * constrainToViewportMargin : screenSize.y * constrainToViewportMargin;

            //�����ܮئb�ù���
            screenPos.x = Mathf.Clamp(
                    screenPos.x,
                    margin + halfBubbleWidth - bubble.anchorMin.x * screenSize.x,
                    -(margin + halfBubbleWidth) - bubble.anchorMax.x * screenSize.x + screenSize.x
            );
            screenPos.y = Mathf.Clamp(
                    screenPos.y,
                    margin + halfBubbleHeight - bubble.anchorMin.y * screenSize.y,
                    -(margin + halfBubbleHeight) - bubble.anchorMax.y * screenSize.y + screenSize.y
            );
        }
        return screenPos;
    }

    public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
    {
        string characterName = dialogueLine.CharacterName;
        CharacterNametext.text = dialogueLine.CharacterName;
        speakerCharacter = !string.IsNullOrEmpty(characterName) ? FindCharacter(characterName) : null;

        tempDialogueLine = dialogueLine.Text.Text;
        int removeEnd = tempDialogueLine.IndexOf(":");
        dialogueLineText.text = tempDialogueLine.Substring(removeEnd + 1);  //�⨤��W�٧R����X��ܤ��e��r

        typewriter.ShowText(dialogueLineText.text);

        //�ݭn�o�椣�MDialogueRunner�|�d��
        onDialogueLineFinished();
    }
    private void Update()
    {
        //�bupdate���p�� �]������i��|����
        if (dialogueBubbleRect.gameObject.activeInHierarchy)
        {
            dialogueBubbleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, dialogueLineText.preferredWidth + 10f);
           characterNameRect .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CharacterNametext.preferredWidth + 10f);
            if (speakerCharacter != null)
            {
                dialogueBubbleRect.anchoredPosition = WorldToAnchoredPosition(dialogueBubbleRect, speakerCharacter.positionWithOffset, bubbleMargin);
            }
            else
            {
               // �p�G���w�q���n���A�h�b������r���W����ܻy���@���w�]��
                    dialogueBubbleRect.anchoredPosition = WorldToAnchoredPosition(dialogueBubbleRect, playerCharacter.positionWithOffset, bubbleMargin);
            }
        }
        if (optionsBubbleRect.gameObject.activeInHierarchy) //�ﶵUI����
        {
            optionsBubbleRect.anchoredPosition = WorldToAnchoredPosition(optionsBubbleRect, playerCharacter.positionWithOffset, bubbleMargin);
        }
    }
    public void DialogueEndSetPlayerBehaviourState()
    {
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.None);
    }
    public void DialogueStartSetPlayerBehaviourState()
    {
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.Talking);
    }
}
