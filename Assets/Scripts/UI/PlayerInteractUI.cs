using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ���a�i�椬���UI
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
                SetTipText("���");
            }
            else if(npcInteractable.isSavePoint)
            {
                SetTipText("�s�ɿ��");
            }
            else if (npcInteractable.isTeleport)
            {
                SetTipText("�ǰe");
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
