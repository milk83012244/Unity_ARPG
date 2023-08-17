using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel���t�d���s����
/// </summary>
public class StartPanel : BasePanel
{
    private static string name = "StartPanel";
    private static string path = "UIPanel/StartPanel";
    public static readonly UIType thisUIType = new UIType(path,name);

    private StartPanelMenu startPanelMenu;

    public StartPanel() : base(thisUIType)
    {
        
    }

    public override void OnStart()
    {
        base.OnStart();
        startPanelMenu = activeObj.GetComponent<StartPanelMenu>();
        //���s�ƥ�
        //UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "Back").onClick.AddListener(Back);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "StartButton").onClick.AddListener(OnStartButtonClicked);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "ContinueButton").onClick.AddListener(OnContinueButtonClicked);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "LoadButton").onClick.AddListener(OnLoadButtonClicked);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "SettingButton").onClick.AddListener(OnSettingButtonClicked);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "ExitButton").onClick.AddListener(OnExitButtonClicked);
    }
    private void OnStartButtonClicked()
    {
        if (startPanelMenu.saveSlotsPanelInstance == null)
            startPanelMenu.InstantiateSaveSlotPanel();

        startPanelMenu.saveSlotsPanelMenu.ActivateMenu(false);

        startPanelMenu.DeactivateMenu();

        Debug.Log("�s�C�� ���}�s�ɼ�");
        //Ū������
        //GameRoot.GetInstance().SceneContralRoot.LoadScene(mainScene.sceneName, mainScene);
    }
    private void OnContinueButtonClicked()
    {
        startPanelMenu.DisableMenuButtons();

        DataPersistenceManager.Instance.SaveGame();

        //�i�J�̫�s�ɪ�����
        //GameRoot.GetInstance().SceneContralRoot.LoadScene(mainScene.sceneName, mainScene);
        Debug.Log("�~��C��Ū���̷s�s��");
    }
    private void OnLoadButtonClicked()
    {
        if (startPanelMenu.saveSlotsPanelInstance == null)
            startPanelMenu.InstantiateSaveSlotPanel();

        startPanelMenu.saveSlotsPanelMenu.ActivateMenu(true);
        startPanelMenu.DeactivateMenu();
        Debug.Log("Ū���s��");
    }
    private void OnSettingButtonClicked()
    {
        Debug.Log("�i�J�]�w");
    }
    private void OnExitButtonClicked()
    {
        Debug.Log("�h�X�C��");
    }
    private void Back()
    {
        GameRoot.GetInstance().UIManagerRoot.Pop(false);
    }
    public override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnDisable()
    {
        Debug.Log($"{name}����");
        base.OnDisable();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
