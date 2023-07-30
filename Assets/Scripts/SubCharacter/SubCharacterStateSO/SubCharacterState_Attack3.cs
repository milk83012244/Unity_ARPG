using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Attack3", fileName = "SubCharacterState_Attack3")]
public class SubCharacterState_Attack3 : SubCharacterState
{
    public override void Enter()
    {
        base.Enter();
        switch (subCharacterController.currentDirectionLeftRight)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Attack3");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Attack3");
                break;
        }
    }
    public override void LogicUpdate()
    {

        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Idle));
        }
    }
}
