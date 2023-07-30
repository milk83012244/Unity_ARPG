using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Attack1", fileName = "SubCharacterState_Attack1")]
public class SubCharacterState_Attack1 : SubCharacterState
{
    public override void Enter()
    {
        base.Enter();
        switch (subCharacterController.currentDirectionLeftRight)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Attack1");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Attack1");
                break;
        }
    }
    public override void LogicUpdate()
    {
        if (CurrentStateTime >= 0.8f)
        {
            if (PlayerState_Attack2.isAttack2)
            {
               stateMachine.SwitchState(typeof(SubCharacterState_Attack2));
            }
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Idle));
        }
    }
}
