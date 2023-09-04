using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// 對話系統用命令管理器 管理場景用Yarn檔案使用的命令
/// </summary>
public class YarnSceneDirector : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
}

