using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    private UIManager uIManager;
    public UIManager UIManagerRoot { get => uIManager; }

    private SceneContral sceneContral;
    public SceneContral SceneContralRoot { get => sceneContral; }

    private static GameRoot instance;

    public static GameRoot GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有UIManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        uIManager = new UIManager();
        sceneContral = new SceneContral();
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        UIManagerRoot.canvsObj = UIUtility.GetInstance().FindCanvas();

        //添加場景
        MainScene mainScene = new MainScene();
        SceneContralRoot.sceneDic.Add(mainScene.sceneName, mainScene);

        //推入第一個面板
        UIManagerRoot.Push(new StartPanel());
    }
}
