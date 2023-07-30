using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格擋狀態
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Guard", fileName = "PlayerState_Guard")]
public class PlayerState_Guard : PlayerState
{
    public override void Enter()
    {
        //如果角色沒有此動作就返回待機
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                stateMachine.SwitchState(typeof(PlayerState_Idle));
                return;
                //break;
        }

        base.Enter();
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":

                if (input.currentDirection == 1)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_GuardPose");
                }
                else if (input.currentDirection == 3)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_GuardPose");
                }
                else
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_GuardPose");
                }
                break;
            case "Lia":
                break;
        }
    }
    public override void Exit()
    {

    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        if (MoCounterCheck.isGuardHit)
        {
            stateMachine.SwitchState(typeof(PlayerState_GuardHit));
        }

        if (!input.PressGuard)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
    public override void PhysicUpdate()
    {

    }
}
