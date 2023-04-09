using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC����
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
    /// �P��L���󤬰�
    /// </summary>
    public void Interact(Transform interactorTranform)
    {
        //��� �Ыع�ܮ�
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
