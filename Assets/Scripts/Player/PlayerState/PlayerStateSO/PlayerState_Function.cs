using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 功能按鈕
/// 負責:播放功能的動畫
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Function", fileName = "PlayerState_Function")]
public class PlayerState_Function : PlayerState
{
    public override void Enter()
    {
        //如果角色沒有此動作就返回待機
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                stateMachine.SwitchState(typeof(PlayerState_Idle));
                return;
        }

        base.Enter();
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                if (input.SwitchFunctionkey1 && input.liaUnlockData.elementUnlockDic[ElementType.Fire] == true)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_SwitchIce");
                }
                else if (input.SwitchFunctionkey2 && input.liaUnlockData.elementUnlockDic[ElementType.Ice] == true)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_SwitchIce");
                }
                else if (input.SwitchFunctionkey3 && input.liaUnlockData.elementUnlockDic[ElementType.Thunder] == true)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_SwitchIce");
                }
                else if (input.SwitchFunctionkey4 && input.liaUnlockData.elementUnlockDic[ElementType.Wind] == true)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_SwitchWind");
                }
                else
                {
                    Debug.Log("屬性未解鎖無法切換動作");
                    return;
                }
                break;
        }
    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        //if (CurrentStateTime >= 0.7f)
        //{

        //}
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
