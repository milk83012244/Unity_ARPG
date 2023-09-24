using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

/// <summary>
/// NPC互動
/// </summary>
public class NPCInteractable : MonoBehaviour, IInteractable
{
    public bool canDialogue; //可對話
    public bool canInteractable; //可與玩家互動
    public bool isSavePoint; //存檔點用
    public bool isTeleport; //傳送點用

    public TipTextObject tipText;

    [SerializeField] private SavePoint savePoint;
    [SerializeField] private TeleportPoint teleportPoint;
    [SerializeField] private DialogueRunner dialogueRunner;
    private Animator animator;

    public string dialogueNode;

    /// <summary>
    /// 與玩家互動
    /// </summary>
    public void Interact(Transform interactorTranform)
    {
        if (GameManager.Instance.CurrentGameState != GameState.Normal)
        {
            tipText.startShowTextCor("當前狀態無法執行互動");
            return;
        }

        if (canDialogue) //對話
        {
            if (dialogueRunner == null)
            {
                Debug.Log("沒有DialogueRunner組件");
                return;
            }
            dialogueRunner.StartDialogue(dialogueNode);
        }
        //先對話完 再互動
        if (canInteractable) //可互動
        {

        }
        if (isSavePoint) //是存檔點
        {
            if (savePoint != null)
            {
                savePoint.OpenSaveLoadMenu();
                GameManager.Instance.SetState(GameState.Paused);
            }
        }
        if (isTeleport) //是傳送點
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
