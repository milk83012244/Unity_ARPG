using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�쥿�b�ϥ�Yarn���ܪ�����
/// </summary>
public class YarnCharacter : MonoBehaviour
{
    public string characterName = "MyName";

    public Vector3 messageBubbleOffset = new Vector3(0f, 3f, 0f);

    public bool offsetUsesRotation = false;

    public bool isOnEnableActive; //�}�Ҫ���ɤ~���U

    public Vector3 positionWithOffset
    {
        get
        {
            if (!offsetUsesRotation)
            {
                return transform.position + messageBubbleOffset;
            }
            else
            {
                return transform.position + transform.TransformPoint(messageBubbleOffset); // �নlocal
            }
        }
    }

    private void OnEnable()
    {
        if (isOnEnableActive)
            YarnCharacterBubbleView.instance.RegisterYarnCharacter(this);
    }
    private void Start()
    {
        if (YarnCharacterBubbleView.instance == null)
        {
            Debug.LogError("�S�����YarnCharacterBubbleView���");
            return;
        }

        if (!isOnEnableActive)
            YarnCharacterBubbleView.instance.RegisterYarnCharacter(this);
    }
    private void OnDisable()
    {
        if (YarnCharacterBubbleView.instance != null)
        {
            YarnCharacterBubbleView.instance.ForgetYarnCharacter(this);
        }
    }
    void OnDestroy()
    {
        if (YarnCharacterBubbleView.instance != null)
        {
            YarnCharacterBubbleView.instance.ForgetYarnCharacter(this);
        }
    }
}
