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
        //只有一般狀態可以開啟隊伍選單
        if (GameManager.Instance.CurrentGameState != GameState.Normal)
        {
            tipTextObject.startShowTextCor("當前狀態無法開啟隊伍選單");
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
        //遊戲暫停狀態
        GameManager.Instance.SetState(GameState.Paused);
    }
}
