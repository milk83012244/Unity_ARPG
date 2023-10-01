using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Attack3", fileName = "PlayerState_Attack3")]
public class PlayerState_Attack3 : PlayerState
{
    public static bool isAttack3;
    public override void Enter()
    {
        isAttack3 = true;
        base.Enter();
        if (input.PressAttack && input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Attack3");
        }
        else if (input.PressAttack && input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Attack3");
        }
        else if (input.PressAttack && input.currentDirection == 2)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Attack3");
        }
        else if (input.PressAttack && input.currentDirection == 4)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Attack3");
        }

        base.SwitchCharacterState(false);
    }
    public override void Exit()
    {
        isAttack3 = false;
    }
    public override void LogicUpdate()
    {
        DamageState();

        if (CurrentStateTime >= 0.7f)
        {
            base.SwitchCharacterState(true);

            if (input.PressDodge)
            {
                stateMachine.SwitchState(typeof(PlayerState_Dodge));
            }
            if (input.PressSkill2)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill2));
            }
        }

        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
