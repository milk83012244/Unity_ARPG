using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家可交互顯示UI
/// </summary>
public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private GameObject containerGameObject;

    private string npcObjectName;

    private void Update()
    {
        if (playerInteract.GetInteractableObject()!=null&& !GameManager.GetInstance().isInteractable)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        if (playerInteract.GetInteractableObject() is NPCInteractable)
        {
            playerInteract.GetInteractableObject().GetGameObject().GetComponentInChildren<PlayerInteractUI>().containerGameObject.gameObject.SetActive(true);
        }

    }
    private void Hide()
    {
        //playerInteract.GetInteractableObject().GetComponentInChildren<PlayerInteractUI>().containerGameObject.gameObject.SetActive(false);
        containerGameObject.gameObject.SetActive(false);
    }
}
