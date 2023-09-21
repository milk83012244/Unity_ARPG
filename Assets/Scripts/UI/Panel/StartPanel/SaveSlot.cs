using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Sirenix.Utilities;

/// <summary>
/// 單一存檔格
/// </summary>
public class SaveSlot : MonoBehaviour
{
    [Header("Profile")] //存檔的目錄名
    [SerializeField] private string profileId = "";

    [Header("Content")] //文字內容
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI saveTimeText;

    [Header("Images")]//圖片內容
    [SerializeField] private Image niruIcon;

    //[Header("Buttons")]
    //[SerializeField] private Button clearButton;

    public bool HasData { get; private set; } = false;

    private Button saveSlotButton;

    private void Awake()
    {
        saveSlotButton = this.GetComponent<Button>();
    }

    /// <summary>
    /// 設定存檔槽的資料
    /// </summary>
    public void SetData(GameData gameData)
    {
        if (gameData == null)
        {
            HasData = false;
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
            niruIcon.enabled = false;
            //clearButton.gameObject.SetActive(false);
        }
        else
        {
            HasData = true;
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            niruIcon.enabled = true;
            //clearButton.gameObject.SetActive(true);

            //處理總遊戲時間文字
            string result = "";
            if (gameData.playTime > 0)
            {
                int index = TimeSpan.FromSeconds(gameData.playTime).ToString().IndexOf(".");
                result = TimeSpan.FromSeconds(gameData.playTime).ToString().Substring(0, index);
            }
            else
            {
                result = "00:00:00";
            }

            saveTimeText.text ="SaveTime: " + gameData.GetSaveTime();
            playTimeText.text = "PlayTime: " + result;
        }
    }
    /// <summary>
    /// 獲得存檔目錄名
    /// </summary>
    public string GetProfileId()
    {
        return this.profileId;
    }
    /// <summary>
    /// 設定按鈕可點擊
    /// </summary>
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        //clearButton.interactable = interactable;
    }
}
