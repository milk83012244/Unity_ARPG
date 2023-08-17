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
            Debug.LogError("�S��GameRoot���");
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
        MainMenu mainMenu = new MainMenu();
        SceneContralRoot.sceneDic.Add(mainMenu.sceneName, mainMenu);

        //���J�Ĥ@�ӭ��O
        UIManagerRoot.Push(new StartPanel());
    }
}
