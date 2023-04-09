using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestTemp : MonoBehaviour
{
    public Button Changebutton;
    public TextMeshProUGUI stateText;

    public GameObject subCharacterParentObj;

    public PlayerStateMachine stateMachine;
    public PlayerCharacterSwitch playerCharacterSwitch;

    public SubCharacterStateMachine subCharacterState;
    public SubCharacterSwitch subCharacterSwitch;
    private void Awake()
    {
        Changebutton.onClick.AddListener(SwitchBattleModeDeBug);  
    }
    private void SwitchBattleModeDeBug()
    {
        if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Normel)
        {
            GameManager.GetInstance().SwitchBattleMode();
            stateText.text = GameManager.GetInstance().CurrentGameState.ToString();
        }
        else if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle)
        {
            GameManager.GetInstance().SwitchNormelMode();
            stateText.text = GameManager.GetInstance().CurrentGameState.ToString();
        }

        if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Normel)
        {
            subCharacterParentObj.SetActive(false);

            playerCharacterSwitch.SwitchMainCharacterInNormal();
            stateMachine.SwitchState(typeof(PlayerState_Idle));

            if (subCharacterParentObj.activeSelf)
            {
                subCharacterSwitch.BattleModeSubCharacterDisable();
            }
        }
        else if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle)
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
