using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// <summary>
/// 遊戲主介面角色頭像管理
/// </summary>
public class MainCharacterIcon : SerializedMonoBehaviour
{
    public PlayerCharacterSwitch playerCharacterSwitch;
    public Image characterIconSlot;
    public Dictionary<string, Sprite> characterIconImages = new Dictionary<string, Sprite>();

    private void OnEnable()
    {
        playerCharacterSwitch.onCharacterSwitch += SetCharacterIcon;
    }
    private void OnDisable()
    {
        playerCharacterSwitch.onCharacterSwitch -= SetCharacterIcon;
    }
    public void SetCharacterIcon(string characterName)
    {
        characterIconSlot.sprite = characterIconImages[characterName];
    }
}
