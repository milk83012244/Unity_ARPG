using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

/// <summary>
/// �C���D���������Y���޲z
/// </summary>
public class MainCharacterIcon : SerializedMonoBehaviour
{
    public PlayerCharacterSwitch playerCharacterSwitch;
    public Image characterIconSlot;
    public Dictionary<string, Sprite> characterIconImages = new Dictionary<string, Sprite>();

    private void OnEnable()
    {
        playerCharacterSwitch.onCharacterSwitch += SetSCharacterIcon;
    }
    private void OnDisable()
    {
        playerCharacterSwitch.onCharacterSwitch -= SetSCharacterIcon;
    }
    public void SetSCharacterIcon(string characterName)
    {
        characterIconSlot.sprite = characterIconImages[characterName];
    }
}
