using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定位正在使用Yarn說話的角色
/// </summary>
public class YarnCharacter : MonoBehaviour
{
    public string characterName = "MyName";

    public Vector3 messageBubbleOffset = new Vector3(0f, 3f, 0f);

    public bool offsetUsesRotation = false;

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
                return transform.position + transform.TransformPoint(messageBubbleOffset); // 轉成local
            }
        }
    }

    private void Start()
    {
        if (YarnCharacterBubbleView.instance == null)
        {
            Debug.LogError("沒有找到YarnCharacterBubbleView實例");
            return;
        }

        YarnCharacterBubbleView.instance.RegisterYarnCharacter(this);
    }
    void OnDestroy()
    {
        if (YarnCharacterBubbleView.instance != null)
        {
            YarnCharacterBubbleView.instance.ForgetYarnCharacter(this);
        }
    }
}
