using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.Utilities;

public class TestTemp : MonoBehaviour
{
    public Button Changebutton;
    public TextMeshProUGUI stateText;

    public PartyDataSO partyData;

    public GameObject subCharacterParentObj;

    public PlayerStateMachine stateMachine;
    public PlayerCharacterSwitch playerCharacterSwitch;

    public SubCharacterStateMachine subCharacterState;
    public SubCharacterSwitch subCharacterSwitch;
    private void Awake()
    {
        Changebutton.onClick.AddListener(SwitchBattleModeDeBug);  
    }
    private void Start()
    {
        
    }
    private void SwitchBattleModeDeBug()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Normal)
        {
            for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
            {
                if (partyData.currentParty[1].IsNullOrWhitespace())
                {
                    Debug.Log("隊伍中無角色 無法切換至戰鬥模式");
                    return;
                }
            }

            GameManager.Instance.SetState(GameState.Battle);
            stateText.text = GameManager.Instance.CurrentGameState.ToString();
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            GameManager.Instance.SetState(GameState.Normal);
            stateText.text = GameManager.Instance.CurrentGameState.ToString();
        }

        if (GameManager.Instance.CurrentGameState == GameState.Normal)
        {
            subCharacterParentObj.SetActive(false);

            playerCharacterSwitch.SwitchMainCharacterInNormal();
            stateMachine.SwitchState(typeof(PlayerState_Idle));

            if (subCharacterParentObj.activeSelf)
            {
                subCharacterSwitch.BattleModeSubCharacterDisable();
            }
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            subCharacterParentObj.SetActive(true);
            subCharacterSwitch.StartPos(stateMachine.transform.position);

            playerCharacterSwitch.BattleModeStartSwitchCharacter();
            stateMachine.SwitchState(typeof(PlayerState_Idle));

            subCharacterSwitch.BattleModeSubCharacterEnable();
            subCharacterState.SwitchState(typeof(SubCharacterState_Idle));
        }
    }
}
