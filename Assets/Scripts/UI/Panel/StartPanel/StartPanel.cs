using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : BasePanel
{
    private static string name = "StartPanel";
    private static string path = "UIPanel/StartPanel";
    public static readonly UIType thisUIType = new UIType(path,name);

    public StartPanel() : base(thisUIType)
    {
        
    }

    public override void OnStart()
    {
        base.OnStart();
        //按鈕事件
        //UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "Back").onClick.AddListener(Back);
        UIUtility.GetInstance().GetOrAddComponentInChild<Button>(activeObj, "Start").onClick.AddListener(StartGame);
    }
    private void StartGame()
    {
        //讀取場景
        MainScene mainScene = new MainScene();
        GameRoot.GetInstance().SceneContralRoot.LoadScene(mainScene.sceneName, mainScene);
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
