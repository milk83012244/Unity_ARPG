using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������������
/// </summary>
public class SceneContral 
{
    public Dictionary<string, SceneBase> sceneDic;

    private static SceneContral instance; 
    public static SceneContral GetInstacce()
    {
        if (instance == null)
        {
            Debug.LogError("�S��SceneContral���");
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
    /// Ū������
    /// </summary>
    /// <param name="sceneName">�ؼг����W</param>
    /// <param name="sceneBase">�ؼг�����</param>
    public void LoadScene(string  sceneName,SceneBase sceneBase)
    {
        if (!sceneDic.ContainsKey(sceneName))
        {
            sceneDic.Add(sceneName, sceneBase);
        }
        //�h�X��e����
        if (sceneDic.ContainsKey(SceneManager.GetActiveScene().name))
        {
            sceneDic[SceneManager.GetActiveScene().name].ExitScene();
        }
        else
        {
            Debug.LogWarning($"SceneContral���r�夤���]�t{SceneManager.GetActiveScene().name}");
        }
        //�����Ҧ����O
        UIManager.GetInstance().Pop(true);

        SceneManager.LoadScene(sceneName);
        sceneBase.EnterScene();
    }
}
