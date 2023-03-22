using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Attack", fileName = "PlayerState_Attack")]
public class PlayerState_Attack : PlayerState
{

    public override void Enter()
    {
        base.Enter();
        if (input.PressAttack && input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Attack1");
        }
        else if (input.PressAttack && input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Attack1");
        }
        else if (input.PressAttack && input.currentDirection == 2)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Attack1");
        }
        else if (input.PressAttack && input.currentDirection == 4)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Attack1");
        }
    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        if (CurrentStateTime >= 0.7f && input.PressAttack)
        {
            stateMachine.SwitchState(typeof(PlayerState_Attack2));
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
