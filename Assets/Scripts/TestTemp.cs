using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTemp : MonoBehaviour
{
    public GameObject subCharacterParentObj;

    public PlayerStateMachine stateMachine;
    public PlayerCharacterSwitch playerCharacterSwitch;

    public SubCharacterStateMachine subCharacterState;
    public SubCharacterSwitch subCharacterSwitch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Normel)
        {
            GameManager.GetInstance().SwitchBattleMode();
        }
        else if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle)
        {
            GameManager.GetInstance().SwitchNormelMode();
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
