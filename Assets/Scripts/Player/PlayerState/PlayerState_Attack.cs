using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attack", fileName = "PlayerState_Attack")]
public class PlayerState_Attack : PlayerState
{
    public override void Enter()
    {
        base.Enter();
        if (input.PressAttack)
        {
            animator.Play(player.currentControlCharacterNames + "_SL_Attack1");
        }
    }
    public override void LogicUpdate()
    {
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
