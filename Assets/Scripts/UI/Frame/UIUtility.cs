using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI�u����
/// </summary>
public class UIUtility 
{
    private static UIUtility instance;

    public static UIUtility GetInstance()
    {
        if (instance == null)
        {
            instance = new UIUtility();
        }
        return instance;
    }
    /// <summary>
    /// ��o��������Canvas��^
    /// </summary>
    /// <returns></returns>
    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (gameObject == null)
        {
            Debug.LogError("�S���b���������Canvas");
            return gameObject;
        }
        return gameObject;
    }

    /// <summary>
    /// ��oPanel���l����
    /// </summary>
    /// <param name="panel">Panel����</param>
    /// <param name="childName">�l����W��</param>
    /// <returns></returns>
    public GameObject FindObjectInChild(GameObject panel,string childName)
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();

        foreach (var item in transforms) //���W�٪�^
        {
            if (item.gameObject.name == childName)
            {
                return item.gameObject;
            }
        }
        Debug.LogWarning($"�b{panel.name}���S�����{childName}����");
        return null;
    }
    /// <summary>
    /// ���w���󪺲ե� �S���h�K�[ �����ܫh��o�ե� ��^
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="gameObject">����</param>
    /// <returns></returns>
    public T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null)
        {
            return gameObject.GetComponent<T>();
        }

        Debug.LogWarning($"{gameObject.name} ����W���s�b�ؼвե�");
        gameObject.AddComponent<T>();
        return gameObject.AddComponent<T>();
    }

    /// <summary>
    /// ���w�l����W�٪��ե� �S���h�K�[ �����ܫh��o�ե� ��^
    /// </summary>
    /// <typeparam name="T">����</typeparam>
    /// <param name="gameObject">����</param>
    /// <param name="childName">�l����W��</param>
    /// <returns></returns>
    public T GetOrAddComponentInChild<T>(GameObject gameObject,string childName)where T : Component
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (var item in transforms) //���W�٪�^
        {
            if (item.gameObject.name == childName)
            {
                if (item.GetComponent<T>() != null)
                {
                    return item.GetComponent<T>();
                }
                else
                {
                    item.gameObject.AddComponent<T>();
                    return item.gameObject.GetComponent<T>();
                }
            }
        }
        Debug.LogWarning($"�b{gameObject.name}���S�����{childName}����");
        return null;
    }
}
