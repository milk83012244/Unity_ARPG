using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Panel類負責按鈕控制
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
        //按鈕事件
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

        Debug.Log("新遊戲 打開存檔槽");
        //讀取場景
        //GameRoot.GetInstance().SceneContralRoot.LoadScene(mainScene.sceneName, mainScene);
    }
    private void OnContinueButtonClicked()
    {
        startPanelMenu.DisableMenuButtons();

        DataPersistenceManager.Instance.SaveGame();

        //進入最後存檔的場景
        //GameRoot.GetInstance().SceneContralRoot.LoadScene(mainScene.sceneName, mainScene);
        Debug.Log("繼續遊戲讀取最新存檔");
    }
    private void OnLoadButtonClicked()
    {
        if (startPanelMenu.saveSlotsPanelInstance == null)
            startPanelMenu.InstantiateSaveSlotPanel();

        startPanelMenu.saveSlotsPanelMenu.ActivateMenu(true);
        startPanelMenu.DeactivateMenu();
        Debug.Log("讀取存檔");
    }
    private void OnSettingButtonClicked()
    {
        Debug.Log("進入設定");
    }
    private void OnExitButtonClicked()
    {
        Debug.Log("退出遊戲");
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
        Debug.Log($"{name}關閉");
        base.OnDisable();
    }

    public override void OnDestory()
    {
        base.OnDestory();
    }
}
