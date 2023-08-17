using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPanelMenu : PanelMenu
{
    //public GameObject firstSelectedPrefab;
    public GameObject saveSlotsPanelPrefab;

    [HideInInspector]public GameObject saveSlotsPanelInstance;

    [Header("Menu Navigation")]
    public SaveSlotsPanelMenu saveSlotsPanelMenu;

    [Header("Menu Button")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button loadButton;

    private Transform canvasParent;
    private void Start()
    {
        canvasParent = this.GetComponentInParent<Canvas>().transform;
        //EventSystem eventSystem = FindObjectOfType<EventSystem>();
        //if (eventSystem != null)
        //{
        //    eventSystem.firstSelectedGameObject = firstSelectedPrefab;
        //}

        DisableButtonsDependingOnData();
    }
    /// <summary>
    /// 沒有存檔時關閉讀取按鈕
    /// </summary>
    private void DisableButtonsDependingOnData()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            continueButton.interactable = false;
            loadButton.interactable = false;
        }
    }
    public void DisableMenuButtons()
    {
        startButton.interactable = false;
        continueButton.interactable = false;
        loadButton.interactable = false;
    }
    /// <summary>
    /// 生成存檔槽菜單
    /// </summary>
    public void InstantiateSaveSlotPanel()
    {
        saveSlotsPanelInstance = Instantiate(saveSlotsPanelPrefab, canvasParent);

        if (saveSlotsPanelInstance != null)
        {
            saveSlotsPanelMenu = saveSlotsPanelInstance.GetComponent<SaveSlotsPanelMenu>();
        }
    }

    public void ActivateMenu()
    {
        this.gameObject.SetActive(true);
        DisableButtonsDependingOnData();
    }
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
}
