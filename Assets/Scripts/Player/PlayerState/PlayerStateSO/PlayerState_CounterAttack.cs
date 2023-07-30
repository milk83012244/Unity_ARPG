using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 反擊狀態
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/CounterAttack", fileName = "PlayerState_CounterAttack")]
public class PlayerState_CounterAttack : PlayerState
{
    public float CounterDashSpeed = 0.5f;
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
                if ( input.currentDirection == 1)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_CounterAttack");
                    player.SetVelocityX(-CounterDashSpeed);
                }
                else if (input.currentDirection == 3)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_CounterAttack");
                    player.SetVelocityX(CounterDashSpeed);
                }
                else if (input.currentDirection == 2)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_CounterAttack");
                    player.SetVelocityY(-CounterDashSpeed);
                }
                else if (input.currentDirection == 4)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_CounterAttack");
                    player.SetVelocityY(CounterDashSpeed);
                }
                break;
            case "Lia":
                break;
        }

        base.SwitchCharacterState(false);
    }
    public override void Exit()
    {

    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        if (IsAnimationFinished)
        {
            base.SwitchCharacterState(true);

            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
    public override void PhysicUpdate()
    {
        
    }
}
