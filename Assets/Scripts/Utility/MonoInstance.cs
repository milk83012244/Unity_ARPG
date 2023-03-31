using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mono單例 用來調用Mono函數
/// </summary>
public class MonoInstance : MonoBehaviour
{
    public static MonoInstance instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public static MonoInstance GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有GameManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }
}
