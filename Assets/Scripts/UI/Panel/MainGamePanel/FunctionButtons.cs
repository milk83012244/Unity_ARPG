using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

/// <summary>
/// 功能按鈕
/// 負責:角色切換時的功能按鈕切換
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
    /// 各角色對應功能按鈕切換
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
