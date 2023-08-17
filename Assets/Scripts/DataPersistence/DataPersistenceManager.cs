using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

/// <summary>
/// �ƾګ��[��(�O�s�PŪ���s��)�޲z��
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
    [SerializeField] private bool disableDataPersistence = false; //�T�μƾګ��[�� ���|�ϥΦs��
    [SerializeField] private bool intializeDataIfNull = false; //�ոե� �b��L�������ժ�����l���ɮ�
    [SerializeField] private bool overrideSelectedProfileID = false; //�δ��զs�ɽƼg�w��ܦs�ɥؿ�
    [SerializeField] private string testSelectedProfileID = "test";//�ոեΦs�ɥؿ��W

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

    private Coroutine autoSaveCoroutine;//�۰ʫO�s��{

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
            Debug.LogError("�X�{���ƪ�DataPersistenceManager");
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        if (disableDataPersistence)
        {
            Debug.LogWarning("�ثe�T�μƾګ��[��");
        }

        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);

        InitializeSelectedProfileID();
    }
    /// <summary>
    /// �R���s�ɥؿ����
    /// </summary>
    public void DeleteProfileData(string profileID)
    {
        //�q���B�z���R���ؿ����
        dataHandler.Delete(profileID);
        //��l�ƿ�ܥؿ�ID
        InitializeSelectedProfileID();
        //Ū���C��
        LoadGame();
    }
    /// <summary>
    /// ��l�ƿ�ܥؿ�ID
    /// </summary>
    private void InitializeSelectedProfileID()
    {
        //��o�̷s���s�ɥؿ�
        this.selectedProfileId = dataHandler.GetMostRecentlyUpdatedProfileID();
        if (overrideSelectedProfileID)
        {
            this.selectedProfileId = testSelectedProfileID;
            Debug.LogWarning("�νոզs�ɥؿ��л\��e��ܦs�ɥؿ� �ոզs�ɥؿ�:" + testSelectedProfileID);
        }
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        Debug.Log("�Ыطs�C���ɮ�");
    }
    public void LoadGame()
    {
        if (disableDataPersistence)
        {
            return;
        }

        //Ū�����B�z�����ɮ�
        this.gameData = dataHandler.Load(selectedProfileId);

        if (this.gameData == null && intializeDataIfNull) //�ոե� �ϥηs�C���ɮ�
        {
            NewGame();
        }

        //�p�G�S����ƫh�Ыطs�C���ɮ�
        if (this.gameData == null)
        {
            Debug.LogWarning("�S���s�ɸ��,�ݭn�b�s�C���Ыؤ@��");
            return;
        }

        //�NŪ�����ƾڱ��e���L�}��
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("�Ұ�Ū���ɮ�,�ؿ�ID: " + selectedProfileId);
    }
    public void SaveGame(bool isAutoSave =false)
    {
        if (disableDataPersistence)
        {
            return;
        }

        if (this.gameData == null)
        {
            Debug.LogWarning("�S���s�ɸ��,�ݭn�b�s�C���Ыؤ@��");
            return;
        }

        //�N�ƾڶǵ���L�}���H��s
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        //�O�s�̫�s�ɪ��ɶ�
        gameData.lastUpdated = System.DateTime.Now.ToBinary();

        //�ϥΤ��B�z���O�s���
        if (isAutoSave)
        {
            dataHandler.Save(gameData, "0"); //�۰ʫO�s�T�w��m
        }
        else
        {
            dataHandler.Save(gameData, selectedProfileId);
        }
        Debug.Log("�ҰʫO�s�ɮ�,�ؿ�ID: " + selectedProfileId);
    }
    /// <summary>
    /// �i�J�U�@�ӳ����ɰ�����
    /// </summary>
    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Debug.Log("�I�s�i�J�����ƥ�");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();

        //�}�Ҧ۰ʫO�s
        //if (autoSaveCoroutine != null)
        //{
        //    StopCoroutine(autoSaveCoroutine);
        //}
        //autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    /// <summary>
    /// �󴫿�ܦs�ɥؿ�ID �åߨ�Ū��
    /// </summary>
    public void LoadChangeSelectedProfileID(string newProfileID)
    {
        this.selectedProfileId = newProfileID;
        LoadGame();
    }
    /// <summary>
    /// �C������ʦs�� ��������x�s��ID
    /// </summary>
    public void SaveChangeSelectedProfileID(string profileID)
    {
        this.selectedProfileId = profileID;
    }
    /// <summary>
    /// ���եΦs�ɫ��s
    /// </summary>
    public void DeBugSaveEvent()
    {
        //�ͦ��ɮ׼Ѫ���
       GameObject  saveSlotsObj = Instantiate(saveSlotsPrefab, canvasParent);
        SaveSlotsPanelMenu saveSlotsPanelMenu = saveSlotsObj.GetComponent<SaveSlotsPanelMenu>();
        //���}�s�ɼ�
        saveSlotsPanelMenu.ActivateMenu(false,true);

        //SaveGame();
    }

    /// <summary>
    /// �M��Ҧ��~��IDataPersistence��MonoBehaviour�}��
    /// </summary>
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    /// <summary>
    /// �ˬd�O�_���C�����
    /// </summary>
    public bool HasGameData()
    {
        return  this.gameData != null;
    }

    /// <summary>
    /// ��o�O�s�ɮשҦ��ؿ�
    /// </summary>
    public Dictionary<string,GameData> GetAllProfilesGameData()
    {
        return dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        yield return Yielders.GetWaitForSeconds(autoSaveTimeSeconds);
        SaveGame(true);
        Debug.Log("�w�۰ʫO�s");
    }
}
