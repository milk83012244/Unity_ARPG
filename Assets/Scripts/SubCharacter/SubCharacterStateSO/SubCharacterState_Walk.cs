using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Walk", fileName = "SubCharacterState_Walk")]
public class SubCharacterState_Walk : SubCharacterState
{
    [SerializeField] float walkSpeed = 5f;
    public override void Enter()
    {
        switch (subCharacterController.currentDirection)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Walk");
                break;
            case 2:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_F_Walk");
                break;
            case 4:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_B_Walk");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Walk");
                break;
        }
    }
    public override void LogicUpdate()
    {
        if ((subCharacterController.Moveing ||( playerInput.MoveX || playerInput.MoveY)) && !subCharacterController.RunCheck())
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Walk));
        }
        else if ((subCharacterController.Moveing || (playerInput.MoveX || playerInput.MoveY)) && subCharacterController.RunCheck())
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Run));
        }
        else
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Idle));
        }
        if (PlayerState_Attack.isAttack1)
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Attack1));
        }
    }
    public override void PhysicUpdate()
    {
        subCharacterController.Following(walkSpeed);
    }
}
