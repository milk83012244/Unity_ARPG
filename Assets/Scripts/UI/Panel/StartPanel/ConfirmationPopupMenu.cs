using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// 彈出確認菜單
/// </summary>
public class ConfirmationPopupMenu : PanelMenu
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    /// <summary>
    /// 打開菜單並設定內容
    /// </summary>
    public void ActiveteMenu(string displayText,UnityAction confirmAction ,UnityAction cancelAction)
    {
        this.gameObject.SetActive(true);

        this.displayText.text = displayText;

        confirmButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        confirmButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            confirmAction();
        });
        cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            cancelAction();
        });
    }
    /// <summary>
    /// 關閉菜單
    /// </summary>
    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
