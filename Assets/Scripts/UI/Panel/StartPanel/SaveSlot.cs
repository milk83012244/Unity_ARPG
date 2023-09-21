using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Sirenix.Utilities;

/// <summary>
/// ��@�s�ɮ�
/// </summary>
public class SaveSlot : MonoBehaviour
{
    [Header("Profile")] //�s�ɪ��ؿ��W
    [SerializeField] private string profileId = "";

    [Header("Content")] //��r���e
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI playTimeText;
    [SerializeField] private TextMeshProUGUI saveTimeText;

    [Header("Images")]//�Ϥ����e
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
    /// �]�w�s�ɼѪ����
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

            //�B�z�`�C���ɶ���r
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
    /// ��o�s�ɥؿ��W
    /// </summary>
    public string GetProfileId()
    {
        return this.profileId;
    }
    /// <summary>
    /// �]�w���s�i�I��
    /// </summary>
    public void SetInteractable(bool interactable)
    {
        saveSlotButton.interactable = interactable;
        //clearButton.interactable = interactable;
    }
}
