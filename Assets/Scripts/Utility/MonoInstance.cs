using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mono��� �Ψӽե�Mono���
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
            Debug.LogError("�S��GameManager���");
            return instance;
        }
        else
        {
            return instance;
        }
    }
}
