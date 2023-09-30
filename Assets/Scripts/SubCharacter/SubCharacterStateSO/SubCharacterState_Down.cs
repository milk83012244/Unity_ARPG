using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Down", fileName = "SubCharacterState_Down")]
public class SubCharacterState_Down : SubCharacterState
{
    public override void Enter()
    {
        base.Enter();
        switch (subCharacterController.currentDirectionLeftRight)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Down");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Down");
                break;
        }
    }
    public override void LogicUpdate()
    {

    }
}
