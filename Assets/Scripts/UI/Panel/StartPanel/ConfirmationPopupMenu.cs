using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// �u�X�T�{���
/// </summary>
public class ConfirmationPopupMenu : PanelMenu
{
    [Header("Components")]
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    /// <summary>
    /// ���}���ó]�w���e
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
    /// �������
    /// </summary>
    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
