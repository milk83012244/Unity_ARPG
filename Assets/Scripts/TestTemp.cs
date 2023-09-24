using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.Utilities;

public class TestTemp : MonoBehaviour
{
    public Button Changebutton;
    public TextMeshProUGUI ChangebuttonText;
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

        if (GameManager.Instance.CurrentGameState == GameState.Normal)
        {
            ChangebuttonText.text = "SwitchBattleMode";
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            ChangebuttonText.text = "SwitchNormalMode";
        }
    }
    private void Update()
    {
        stateText.text = GameManager.Instance.CurrentGameState.ToString();
    }
    public void SwitchBattleModeDeBug()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Paused)
            return;

        if (GameManager.Instance.CurrentGameState == GameState.Normal)
        {
            for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
            {
                if (partyData.currentParty[1].IsNullOrWhitespace())
                {
                    Debug.Log("����L���� �L�k�����ܾ԰��Ҧ�");
                    return;
                }
            }

            GameManager.Instance.SetState(GameState.Battle);
            ChangebuttonText.text = "SwitchNormalMode";
        }
        else if (GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            GameManager.Instance.SetState(GameState.Normal);
            ChangebuttonText.text = "SwitchBattleMode";
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
