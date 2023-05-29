using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Attack2", fileName = "PlayerState_Attack2")]
public class PlayerState_Attack2 : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        if (input.PressAttack && input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Attack2");
        }
        else if (input.PressAttack && input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Attack2");
        }
        else if (input.PressAttack && input.currentDirection == 2)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Attack2");
        }
        else if (input.PressAttack && input.currentDirection == 4)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Attack2");
        }
    }
    public override void LogicUpdate()
    {
        if (CurrentStateTime >= 0.7f)
        {
            if (input.PressAttack && characterStats.characterData[characterStats.currentCharacterID].PlayerNormalAttackCount>=3)
            {
                stateMachine.SwitchState(typeof(PlayerState_Attack3));
            }
            if (input.PressSkill1)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill1));
            }
            if (input.PressSkill2)
            {
                stateMachine.SwitchState(typeof(PlayerState_Skill2));
            }
            if (input.PressDodge)
            {
                stateMachine.SwitchState(typeof(PlayerState_Dodge));
            }
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
