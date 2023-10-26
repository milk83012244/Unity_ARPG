using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SaveSlotsPanelMenu : PanelMenu
{
    public TextMeshProUGUI title;
    [Header("Menu Navigation")]
    public StartPanelMenu startPanelMenu;

    [Header("Menu Buttons")]
    [SerializeField] private Button backButton;

    [Header("Confirmation Popup")]
    [SerializeField] private GameObject confirmationPopupMenuPrefab;
    private ConfirmationPopupMenu confirmationPopupMenu;

    private SaveSlot[] saveSlots;

    private bool isLoadingGame = false;
    private bool isSavingGame = false;
    private Transform canvasParent;
    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }
    private void Start()
    {
        canvasParent = GetComponentInParent<Canvas>().transform;

        GameObject confirmationPopupMenuObj = Instantiate(confirmationPopupMenuPrefab, canvasParent);
        confirmationPopupMenu = confirmationPopupMenuObj.GetComponent<ConfirmationPopupMenu>();

        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(this.gameObject, "BackButton").onClick.AddListener(OnBackButtonClicked);
    }

    /// <summary>
    /// 點擊存檔槽
    /// </summary>
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        if (GameManager.Instance.CurrentGameState == GameState.InMenu)
        {
            DisableMenuButtons();
        }

        //讀取檔案
        if (!isSavingGame)
        {
            if (isLoadingGame)
            {
                DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                GameManager.Instance.MenuStartGame();
                SaveGameAndLoadScene();
            }
            else if (saveSlot.HasData)//儲存槽有資料的情況
            {
                confirmationPopupMenu.ActiveteMenu("將會讀取此存檔,要繼續嗎?",
                    //按下Yes
                    () =>
                    {
                        GameManager.Instance.MenuStartGame();
                        DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                        DataPersistenceManager.Instance.NewGame();
                        GameManager.Instance.isNewGame = true;
                        SaveGameAndLoadScene();
                    },
                    //按下Cancel
                    () =>
                    {
                        this.ActivateMenu(isLoadingGame);
                    }
                    );
            }
            else //在空的儲存槽創建新遊戲
            {
                DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                DataPersistenceManager.Instance.NewGame();
                GameManager.Instance.MenuStartGame();
                GameManager.Instance.isNewGame = true;
                SaveGameAndLoadScene();
            }
        }
        //保存檔案
        if (isSavingGame)
        {
            if (saveSlot.HasData)
            {
                confirmationPopupMenu.ActiveteMenu("將會覆蓋此儲存槽的存檔,要繼續嗎?",
              //按下Yes
             () =>
             {
                 if (GameManager.Instance.inMenu)
                     GameManager.Instance.MenuStartGame();

                 DataPersistenceManager.Instance.SaveChangeSelectedProfileID(saveSlot.GetProfileId());
             DataPersistenceManager.Instance.SaveGame();
              },
            //按下Cancel
            () =>
            {
                this.ActivateMenu(false,true);
            }
            );
            }
            else
            {
                //獲得儲存格的目錄ID
                DataPersistenceManager.Instance.SaveChangeSelectedProfileID(saveSlot.GetProfileId());
                //保存檔案
                DataPersistenceManager.Instance.SaveGame();
            }
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
    private void SaveGameAndLoadScene()
    {
        DataPersistenceManager.Instance.SaveGame();

        //異步加載場景
        //SceneManager.LoadSceneAsync("MainScene");
        SceneManager.LoadSceneAsync("DemoMainScene");
    }

    /// <summary>
    /// 清除存檔按鈕
    /// </summary>
    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActiveteMenu("確定刪除存檔?",
            //Yes
            () =>
            {
                DataPersistenceManager.Instance.DeleteProfileData(saveSlot.GetProfileId());
                ActivateMenu(isLoadingGame);
            },
            //Cancel
            () =>
            {
                ActivateMenu(isLoadingGame);
            }
            );
    }

    /// <summary>
    /// 打開菜單並設定儲存槽內容
    /// </summary>
    public void ActivateMenu(bool isLoadingGame, bool isSavingGame = false)
    {
        if (isLoadingGame)
            title.text = "Load";
        else
            title.text = "Save";

        this.gameObject.SetActive(true);

        this.isSavingGame = isSavingGame;
        this.isLoadingGame = isLoadingGame;

        if (startPanelMenu == null)
            startPanelMenu = FindObjectOfType<StartPanelMenu>();

        //讀取所有檔案目錄
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        backButton.interactable = true;

        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            //讀取遊戲時關閉空的存檔槽
            if (profileData == null && isLoadingGame)
            {
                saveSlot.SetInteractable(false);
            }
            else
            {
                saveSlot.SetInteractable(true);
                if (firstSelected.Equals(backButton.gameObject))
                {
                    firstSelected = saveSlot.gameObject;
                }
            }
        }

        Button firstSelectedButton = firstSelected.GetComponent<Button>();
        this.SetFirstSelected(firstSelectedButton); //設定預設按鍵選擇
    }
    /// <summary>
    /// 關閉菜單
    /// </summary>
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        if (GameManager.Instance.CurrentGameState == GameState.InMenu)
        {
            startPanelMenu.ActivateMenu();
        }
        if (GameManager.Instance.CurrentGameState == GameState.Paused)
        {
            GameManager.Instance.SetState(GameState.Normal);
        }

        DeactivateMenu();
    }
    /// <summary>
    /// 關閉菜單按鈕
    /// </summary>
    private void DisableMenuButtons()
    {
        foreach (SaveSlot saveSlot in saveSlots)
        {
            saveSlot.SetInteractable(false);
        }
        backButton.interactable = false;
    }

}
