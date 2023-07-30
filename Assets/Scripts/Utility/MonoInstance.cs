using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mono��� �Ψӽե�Mono���
/// </summary>
public class MonoInstance : MonoBehaviour
{
    private static MonoInstance instance;
    public static MonoInstance Instance
    {
        get
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
    }
    //public static MonoInstance GetInstance()
    //{
    //    if (instance == null)
    //    {
    //        Debug.LogError("�S��MonoInstance���");
    //        return instance;
    //    }
    //    else
    //    {
    //        return instance;
    //    }
    //}
}
