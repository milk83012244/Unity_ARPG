using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/GuardHit", fileName = "PlayerState_GuardHit")]
public class PlayerState_GuardHit : PlayerState
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
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_GuardHit");
                }
                else if (input.currentDirection == 3)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_GuardHit");
                }
                else
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_GuardHit");
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

            stateMachine.SwitchState(typeof(PlayerState_CounterAttack));
        }

    }
    public override void PhysicUpdate()
    {

    }
}
