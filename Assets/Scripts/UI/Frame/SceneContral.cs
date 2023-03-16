using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 場景切換控制
/// </summary>
public class SceneContral 
{
    public Dictionary<string, SceneBase> sceneDic;

    private static SceneContral instance; 
    public static SceneContral GetInstacce()
    {
        if (instance == null)
        {
            Debug.LogError("沒有SceneContral實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }
    public SceneContral()
    {
        instance = this;
        sceneDic = new Dictionary<string, SceneBase>();
    }

    /// <summary>
    /// 讀取場景
    /// </summary>
    /// <param name="sceneName">目標場景名</param>
    /// <param name="sceneBase">目標場景類</param>
    public void LoadScene(string  sceneName,SceneBase sceneBase)
    {
        if (!sceneDic.ContainsKey(sceneName))
        {
            sceneDic.Add(sceneName, sceneBase);
        }
        //退出當前場景
        if (sceneDic.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneDic[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.LogWarning($"SceneContral的字典中不包含{SceneManager.GetActiveScene().name}");
        }
        //關閉所有面板
        UIManager.GetInstance().Pop(true);

        SceneManager.LoadScene(sceneName);
        sceneBase.EnterScene();
    }
}
