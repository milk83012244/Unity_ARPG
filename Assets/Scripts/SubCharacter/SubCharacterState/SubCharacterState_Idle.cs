using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Idle", fileName = "SubCharacterState_Idle")]
public class SubCharacterState_Idle : SubCharacterState
{
    public override void Enter()
    {
        switch (playerInput.currentDirection)
        {
            case 1:
            case 2:
            case 4:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_BattleIdle");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_BattleIdle");
                break;
        }

    }
    public override void LogicUpdate()
    {
        if (subCharacterController.FollowingCheck())
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Walk));
        }
    }
    public override void PhysicUpdate()
    {
        
    }
}
