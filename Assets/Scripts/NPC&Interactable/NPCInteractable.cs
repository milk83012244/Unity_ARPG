using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC����
/// </summary>
public class NPCInteractable : MonoBehaviour, IInteractable
{
    public bool canDialogue; //�i���
    public bool canInteractable; //�i�P���a����
    public bool isSavePoint; //�s���I��
    public bool isTeleport; //�ǰe�I��

    public TipTextObject tipText;

    [SerializeField] private SavePoint savePoint;
    [SerializeField] private TeleportPoint teleportPoint;
    [SerializeField] private DialogueRunner dialogueRunner;
    private Animator animator;

    public string dialogueNode;

    /// <summary>
    /// �P���a����
    /// </summary>
    public void Interact(Transform interactorTranform)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Normal)
        {
            tipText.startShowTextCor("��e���A�L�k���椬��");
            return;
        }

        if (canDialogue) //���
        {
            if (dialogueRunner == null)
            {
                Debug.Log("�S��DialogueRunner�ե�");
                return;
            }
            dialogueRunner.StartDialogue(dialogueNode);
        }
        //����ܧ� �A����
        if (canInteractable) //�i����
        {

        }
        if (isSavePoint) //�O�s���I
        {
            if (savePoint != null)
            {
                savePoint.OpenSaveLoadMenu();
                GameManager.Instance.SetState(GameState.Paused);
            }
        }
        if (isTeleport) //�O�ǰe�I
        {
            teleportPoint.Teleport(PlayerController.GetInstance().transform);
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
