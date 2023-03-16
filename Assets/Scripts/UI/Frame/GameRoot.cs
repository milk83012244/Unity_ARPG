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
            Debug.LogError("�S��UIManager���");
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

        //�K�[����
        MainScene mainScene = new MainScene();
        SceneContralRoot.sceneDic.Add(mainScene.sceneName, mainScene);

        //���J�Ĥ@�ӭ��O
        UIManagerRoot.Push(new StartPanel());
    }
}
