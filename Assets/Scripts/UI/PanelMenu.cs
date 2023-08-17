using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 設定Panel之間共同的功能
/// </summary>
public class PanelMenu : MonoBehaviour
{
    [Header("First Selected Button")]
    [SerializeField] private Button firstSelected;

    protected virtual void OnEnable()
    {
        SetFirstSelected(firstSelected);
    }
    /// <summary>
    /// 設定按鍵預設選擇UI按鈕
    /// </summary>
    public void SetFirstSelected(Button firstSelectedButton)
    {
        firstSelectedButton.Select();
    }
}
