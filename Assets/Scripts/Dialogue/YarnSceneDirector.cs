using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// ��ܨt�ΥΩR�O�޲z�� �޲z������Yarn�ɮרϥΪ��R�O
/// </summary>
public class YarnSceneDirector : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
}

