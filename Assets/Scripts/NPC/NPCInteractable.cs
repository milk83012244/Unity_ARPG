using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC����
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
    /// �P���a����
    /// </summary>
    public void Interact(Transform interactorTranform,bool Dialogue=false)
    {
        if (Dialogue) //���
        {
            if (dialogueRunner == null)
            {
                Debug.Log("�S��DialogueRunner�ե�");
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
