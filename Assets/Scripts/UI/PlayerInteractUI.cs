using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// 玩家可交互顯示UI
/// </summary>
public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private TextMeshProUGUI tipText;

    private NPCInteractable npcInteractable;
    private string npcObjectName;

    private void Start()
    {

    }
    private void Update()
    {
        if (playerInteract.GetInteractableObject()!=null && GameManager.Instance.GetCurrentPlayerBehaviourState() == PlayerBehaviourState.None)
        {
            npcInteractable = playerInteract.GetInteractableObject()  as NPCInteractable;
            if (npcInteractable.canDialogue)
            {
                SetTipText("對話");
            }
            else if(npcInteractable.isSavePoint)
            {
                SetTipText("存檔選單");
            }
            else if (npcInteractable.isTeleport)
            {
                SetTipText("傳送");
            }
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void SetTipText(string content)
    {
        tipText.text = content;
    }

    private void Show()
    {
        if (playerInteract.GetInteractableObject() is NPCInteractable)
        {
            //playerInteract.GetInteractableObject().GetGameObject().GetComponentInChildren<PlayerInteractUI>().containerGameObject.gameObject.SetActive(true);
            containerGameObject.gameObject.SetActive(true);
        }

    }
    private void Hide()
    {
        //playerInteract.GetInteractableObject().GetComponentInChildren<PlayerInteractUI>().containerGameObject.gameObject.SetActive(false);
        containerGameObject.gameObject.SetActive(false);
    }
}
