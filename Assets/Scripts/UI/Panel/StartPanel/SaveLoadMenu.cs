using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SaveLoadMenu : MonoBehaviour
{
    [SerializeField] private Button loadButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button cancelButton;

    /// <summary>
    /// 打開菜單並設定內容以及註冊事件
    /// </summary>
    public void ActiveteMenu(UnityAction saveAction, UnityAction loadAction)
    {
        this.gameObject.SetActive(true);

        cancelButton.onClick.RemoveAllListeners();

        saveButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            saveAction();
        });
        loadButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            loadAction();
        });
        cancelButton.onClick.AddListener(() =>
        {
            DeactivateMenu();
            GameManager.Instance.SetState(GameState.Normal);
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
