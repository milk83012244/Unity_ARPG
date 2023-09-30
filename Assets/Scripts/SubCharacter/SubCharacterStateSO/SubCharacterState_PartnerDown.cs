using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/PartnerDown", fileName = "SubCharacterState_PartnerDown")]
public class SubCharacterState_PartnerDown : SubCharacterState
{
    public override void Enter()
    {
        base.Enter();
        switch (subCharacterController.currentDirectionLeftRight)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_PartnerDown");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_PartnerDown");
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
