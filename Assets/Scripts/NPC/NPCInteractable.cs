using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC互動
/// </summary>
public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform dialoguePos;
    [SerializeField] private DialogueRunner dialogueRunner;
    private Animator animator;
    private void Awake()
    {
        
    }
    /// <summary>
    /// 與其他物件互動
    /// </summary>
    public void Interact(Transform interactorTranform)
    {
        //對話 創建對話框
        DialogueBubbleCreate.Create(dialogueRunner,this.transform, dialoguePos.localPosition +new Vector3(0,0.25f,0), "TestYarnScript");
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }
}
