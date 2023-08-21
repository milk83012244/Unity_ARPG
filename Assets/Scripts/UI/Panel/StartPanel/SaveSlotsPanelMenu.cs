using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveSlotsPanelMenu : PanelMenu
{
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
    /// �I���s�ɼ�
    /// </summary>
    public void OnSaveSlotClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();
        //Ū���ɮ�
        if (!isSavingGame)
        {
            if (isLoadingGame)
            {
                DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                SaveGameAndLoadScene();
            }
            else if (saveSlot.HasData)//�Ыطs�C�����O�x�s�Ѧ���ƪ����p
            {
                confirmationPopupMenu.ActiveteMenu("�N�|�л\���x�s�Ѫ��s��,�n�~���?",
                    //���UYes
                    () =>
                    {
                        DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                        DataPersistenceManager.Instance.NewGame();
                        SaveGameAndLoadScene();
                    },
                    //���UCancel
                    () =>
                    {
                        this.ActivateMenu(isLoadingGame);
                    }
                    );
            }
            else //�b�Ū��x�s�ѳЫطs�C��
            {
                DataPersistenceManager.Instance.LoadChangeSelectedProfileID(saveSlot.GetProfileId());
                DataPersistenceManager.Instance.NewGame();
                SaveGameAndLoadScene();
            }
        }
        //�O�s�ɮ�
        if (isSavingGame)
        {
            if (saveSlot.HasData)
            {
                confirmationPopupMenu.ActiveteMenu("�N�|�л\���x�s�Ѫ��s��,�n�~���?",
              //���UYes
             () =>
             {
             DataPersistenceManager.Instance.SaveChangeSelectedProfileID(saveSlot.GetProfileId());
             DataPersistenceManager.Instance.SaveGame();
              },
            //���UCancel
            () =>
            {
                this.ActivateMenu(false,true);
            }
            );
            }
            else
            {
                //��o�x�s�檺�ؿ�ID
                DataPersistenceManager.Instance.SaveChangeSelectedProfileID(saveSlot.GetProfileId());
                //�O�s�ɮ�
                DataPersistenceManager.Instance.SaveGame();
            }
            Destroy(this.gameObject);
            //this.gameObject.SetActive(false);
        }
    }
    private void SaveGameAndLoadScene()
    {
        DataPersistenceManager.Instance.SaveGame();

        //���B�[������
        SceneManager.LoadSceneAsync("MainScene");
    }

    /// <summary>
    /// �M���s�ɫ��s
    /// </summary>
    public void OnClearClicked(SaveSlot saveSlot)
    {
        DisableMenuButtons();

        confirmationPopupMenu.ActiveteMenu("�T�w�R���s��?",
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
    /// ���}���ó]�w�x�s�Ѥ��e
    /// </summary>
    public void ActivateMenu(bool isLoadingGame, bool isSavingGame = false)
    {
        this.gameObject.SetActive(true);

        this.isSavingGame = isSavingGame;
        this.isLoadingGame = isLoadingGame;

        if (startPanelMenu == null)
            startPanelMenu = FindObjectOfType<StartPanelMenu>();

        //Ū���Ҧ��ɮץؿ�
        Dictionary<string, GameData> profilesGameData = DataPersistenceManager.Instance.GetAllProfilesGameData();

        backButton.interactable = true;

        GameObject firstSelected = backButton.gameObject;
        foreach (SaveSlot saveSlot in saveSlots)
        {
            GameData profileData = null;
            profilesGameData.TryGetValue(saveSlot.GetProfileId(), out profileData);
            saveSlot.SetData(profileData);
            //Ū���C���������Ū��s�ɼ�
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
        this.SetFirstSelected(firstSelectedButton); //�]�w�w�]������
    }
    /// <summary>
    /// �������
    /// </summary>
    public void DeactivateMenu()
    {
        this.gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        startPanelMenu.ActivateMenu();
        DeactivateMenu();
    }
    /// <summary>
    /// ���������s
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