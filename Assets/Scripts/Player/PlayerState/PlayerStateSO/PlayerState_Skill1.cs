using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Skill1", fileName = "PlayerState_Skill1")]
public class PlayerState_Skill1 : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        if (input.PressSkill1 && input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Skill1");
        }
        else if (input.PressSkill1 && input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Skill1");
        }
        else if (input.PressSkill1 && input.currentDirection == 2)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Skill1");
        }
        else if (input.PressSkill1 && input.currentDirection == 4)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Skill1");
        }
        //ßﬁØ‡CD
        playerCooldownController.Skill1CooldownTrigger.Invoke(playerCharacterSwitch.currentControlCharacterNamesSB.ToString());
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
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
