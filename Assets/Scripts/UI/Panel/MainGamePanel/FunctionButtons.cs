using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

/// <summary>
/// �\����s
/// �t�d:��������ɪ��\����s����
/// </summary>
public class FunctionButtons : SerializedMonoBehaviour
{
    public Dictionary<string, GameObject> characterFunctionButtons = new Dictionary<string, GameObject>();

    public PlayerCharacterSwitch playerCharacterSwitch;

    private void OnEnable()
    {
        playerCharacterSwitch.onCharacterSwitch += FunctionButtonSwitcher;
    }
    private void OnDisable()
    {
        playerCharacterSwitch.onCharacterSwitch -= FunctionButtonSwitcher;
    }

    /// <summary>
    /// �U��������\����s����
    /// </summary>
    public void FunctionButtonSwitcher(string characterName)
    {
        foreach (KeyValuePair<string,GameObject> item in characterFunctionButtons)
        {
            if (item.Key != characterName)
            {
                characterFunctionButtons[item.Key].SetActive(false);
            }
        }

        switch (characterName)
        {
            case "Niru":
                break;
            case "Mo":
                break;
            case "Lia":
                characterFunctionButtons[characterName].SetActive(true);
                break;
        }
    }
}
