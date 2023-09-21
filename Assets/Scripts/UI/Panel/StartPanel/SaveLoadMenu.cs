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
    /// ���}���ó]�w���e�H�ε��U�ƥ�
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
    /// �������
    /// </summary>
    private void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
