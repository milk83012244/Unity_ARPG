using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public TipTextObject tipTextObject;
    [SerializeField]private GameObject partyPanelPrefab;
    private GameObject partyPanelInstance;

    public Transform partyPanelParent;

    public Button partyButton;

    private void Awake()
    {
        partyButton.onClick.AddListener(PartyButtonClick);
    }
    public void PartyButtonClick()
    {
        //�u���@�몬�A�i�H�}�Ҷ�����
        if (GameManager.Instance.CurrentGameState != GameState.Normal)
        {
            tipTextObject.startShowTextCor("��e���A�L�k�}�Ҷ�����");
            return;
        }

        if (partyPanelInstance == null)
        {
            partyPanelInstance = Instantiate(partyPanelPrefab, partyPanelParent);
        }
        else
        {
            partyPanelInstance.SetActive(true);
        }
        //�C���Ȱ����A
        GameManager.Instance.SetState(GameState.Paused);
    }
}
