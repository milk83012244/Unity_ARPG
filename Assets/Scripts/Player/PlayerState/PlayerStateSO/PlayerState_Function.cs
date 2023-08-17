using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �\����s
/// �t�d:����\�઺�ʵe
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Function", fileName = "PlayerState_Function")]
public class PlayerState_Function : PlayerState
{
    public override void Enter()
    {
        //�p�G����S�����ʧ@�N��^�ݾ�
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
                    Debug.Log("�ݩʥ�����L�k�����ʧ@");
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
