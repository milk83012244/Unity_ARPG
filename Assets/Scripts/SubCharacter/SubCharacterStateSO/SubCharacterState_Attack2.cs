using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Attack2", fileName = "SubCharacterState_Attack2")]
public class SubCharacterState_Attack2 : SubCharacterState
{
    public override void Enter()
    {
        base.Enter();
        switch (subCharacterController.currentDirectionLeftRight)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Attack2");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Attack2");
                break;
        }
    }
    public override void LogicUpdate()
    {
        if (CurrentStateTime >= 0.8f)
        {
            if (PlayerState_Attack3.isAttack3)
            {
                stateMachine.SwitchState(typeof(SubCharacterState_Attack3));
            }
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Idle));
        }
    }
}
