using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTemp : MonoBehaviour
{
    public PlayerStateMachine stateMachine;
    public PlayerCharacterSwitch playerCharacterSwitch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Normel)
        {
            GameManager.GetInstance().SwitchBattleMode();
            playerCharacterSwitch.BattleModeStartSwitchCharacter();
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        else if (GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle)
        {
            GameManager.GetInstance().SwitchNormelMode();
            playerCharacterSwitch.SwitchMainCharacterInNormal();
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
