using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI工具類
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
    /// 獲得場景中的Canvas返回
    /// </summary>
    /// <returns></returns>
    public GameObject FindCanvas()
    {
        GameObject gameObject = GameObject.FindObjectOfType<Canvas>().gameObject;
        if (gameObject == null)
        {
            Debug.LogError("沒有在場景中找到Canvas");
            return gameObject;
        }
        return gameObject;
    }

    /// <summary>
    /// 獲得Panel的子物件
    /// </summary>
    /// <param name="panel">Panel物件</param>
    /// <param name="childName">子物件名稱</param>
    /// <returns></returns>
    public GameObject FindObjectInChild(GameObject panel,string childName)
    {
        Transform[] transforms = panel.GetComponentsInChildren<Transform>();

        foreach (var item in transforms) //比對名稱返回
        {
            if (item.gameObject.name == childName)
            {
                return item.gameObject;
            }
        }
        Debug.LogWarning($"在{panel.name}中沒有找到{childName}物體");
        return null;
    }
    /// <summary>
    /// 指定物件的組件 沒有則添加 有的話則獲得組件 返回
    /// </summary>
    /// <typeparam name="T">類型</typeparam>
    /// <param name="gameObject">物件</param>
    /// <returns></returns>
    public T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        if (gameObject.GetComponent<T>() != null)
        {
            return gameObject.GetComponent<T>();
        }

        Debug.LogWarning($"{gameObject.name} 物件上不存在目標組件");
        gameObject.AddComponent<T>();
        return gameObject.AddComponent<T>();
    }

    /// <summary>
    /// 指定子物件名稱的組件 沒有則添加 有的話則獲得組件 返回
    /// </summary>
    /// <typeparam name="T">類型</typeparam>
    /// <param name="gameObject">物件</param>
    /// <param name="childName">子物件名稱</param>
    /// <returns></returns>
    public T GetOrAddComponentInChild<T>(GameObject gameObject,string childName)where T : Component
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();

        foreach (var item in transforms) //比對名稱返回
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
        Debug.LogWarning($"在{gameObject.name}中沒有找到{childName}物體");
        return null;
    }
}
