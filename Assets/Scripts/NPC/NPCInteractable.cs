using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC互動
/// </summary>
public class NPCInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueRunner dialogueRunner;
    private Animator animator;

    public string dialogueNode;
    private void Awake()
    {
        
    }
    /// <summary>
    /// 與玩家互動
    /// </summary>
    public void Interact(Transform interactorTranform,bool Dialogue=false)
    {
        if (Dialogue) //對話
        {
            if (dialogueRunner == null)
            {
                Debug.Log("沒有DialogueRunner組件");
                return;
            }
            dialogueRunner.StartDialogue(dialogueNode);
        }
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
