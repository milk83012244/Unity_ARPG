using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// 數據持久化(保存與讀取存檔)管理器
/// </summary>
public class DataPersistenceManager : MonoBehaviour
{
    private static DataPersistenceManager instance;
    public static DataPersistenceManager Instance
    {
        get
        {
            return instance;
        }
    }

    [Header("Debugging")]
    [SerializeField] private bool disableDataPersistence = false; //禁用數據持久化 不會使用存檔
    [SerializeField] private bool intializeDataIfNull = false; //調試用 在其他場景測試直接初始化檔案
    [SerializeField] private bool overrideSelectedProfileID = false; //用測試存檔複寫已選擇存檔目錄
    [SerializeField] private string testSelectedProfileID = "test";//調試用存檔目錄名

    [SerializeField] private Transform canvasParent;
     public GameObject saveSlotsPrefab;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    [Header("Auto Saving Configuration")]
    [SerializeField] private float autoSaveTimeSeconds = 60f;

    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;

    private string selectedProfileId = "";

    private Coroutine autoSaveCoroutine;//自動保存協程

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("出現重複的DataPersistenceManager");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("目前禁用數據持久化");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileID();
    }
    /// <summary>
    /// 刪除存檔目錄資料
    /// </summary>
    public void DeleteProfileData(string profileID)
    {
        //從文件處理器刪除目錄資料
        dataHandler.Delete(profileID);
        //初始化選擇目錄ID
        InitializeSelectedProfileID();
        //讀取遊戲
        LoadGame();
    }
    /// <summary>
    /// 初始化選擇目錄ID
    /// </summary>
    private void InitializeSelectedProfileID()
    {
        //獲得最新的存檔目錄
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileID();
        if (overrideSelectedProfileID)
        {
            this.selectedProfileId = testSelectedProfileID;
            Debug.LogWarning("用調試存檔目錄覆蓋當前選擇存檔目錄 調試存檔目錄:" + testSelectedProfileID);
        }
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        Debug.Log("創建新遊戲檔案");
    }
    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        //讀取文件處理器的檔案
        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && intializeDataIfNull) //調試用 使用新遊戲檔案
        {
            NewGame();
        }

        //如果沒有資料則創建新遊戲檔案
        if (this.gameData == null)
        {
            Debug.LogWarning("沒有存檔資料,需要在新遊戲創建一個");
            return;
        }

        //將讀取的數據推送到其他腳本
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("啟動讀取檔案,目錄ID: " + selectedProfileId);
    }
    public void SaveGame(bool isAutoSave =false)
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("沒有存檔資料,需要在新遊戲創建一個");
            return;
        }

        //將數據傳給其他腳本以更新
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        //保存最後存檔的時間
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        //使用文件處理器保存資料
        if (isAutoSave)
        {
            dataHandler.Save(gameData, "0"); //自動保存固定位置
        }
        else
        {
            dataHandler.Save(gameData, selectedProfileId);
        }
        Debug.Log("啟動保存檔案,目錄ID: " + selectedProfileId);
    }
    /// <summary>
    /// 進入下一個場景時做什麼
    /// </summary>
    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Debug.Log("呼叫進入場景事件");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

        //開啟自動保存
        //if (autoSaveCoroutine != null)
        //{
        //    StopCoroutine(autoSaveCoroutine);
        //}
        //autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    /// <summary>
    /// 更換選擇存檔目錄ID 並立刻讀取
    /// </summary>
    public void LoadChangeSelectedProfileID(string newProfileID)
    {
        this.selectedProfileId = newProfileID;
        LoadGame();
    }
    /// <summary>
    /// 遊戲中手動存檔 控制切換儲存槽ID
    /// </summary>
    public void SaveChangeSelectedProfileID(string profileID)
    {
        this.selectedProfileId = profileID;
    }
    /// <summary>
    /// 測試用存檔按鈕
    /// </summary>
    public void DeBugSaveEvent()
    {
        //生成檔案槽物件
       GameObject  saveSlotsObj = Instantiate(saveSlotsPrefab, canvasParent);
        SaveSlotsPanelMenu saveSlotsPanelMenu = saveSlotsObj.GetComponent<SaveSlotsPanelMenu>();
        //打開存檔槽
        saveSlotsPanelMenu.ActivateMenu(false,true);

        //SaveGame();
    }

    /// <summary>
    /// 尋找所有繼承IDataPersistence的MonoBehaviour腳本
    /// </summary>
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    /// <summary>
    /// 檢查是否有遊戲資料
    /// </summary>
    public bool HasGameData()
    {
        return  this.gameData != null;
    }

    /// <summary>
    /// 獲得保存檔案所有目錄
    /// </summary>
    public Dictionary<string,GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        yield return Yielders.GetWaitForSeconds(autoSaveTimeSeconds);
        SaveGame(true);
        Debug.Log("已自動保存");
    }
}
