using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/SubCharacterState/Run", fileName = "SubCharacterState_Run")]
public class SubCharacterState_Run : SubCharacterState
{
    [SerializeField] float runSpeed = 5f;

    public override void Enter()
    {
        switch (subCharacterController.currentDirection)
        {
            case 1:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SR_Run");
                break;
            case 2:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_F_Run");
                break;
            case 4:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_B_Run");
                break;
            case 3:
                animator.Play(subCharacterSwitch.currentSubCharacterNamesSB.ToString() + "_SL_Run");
                break;
        }
    }
    public override void LogicUpdate()
    {
        if ((subCharacterController.Moveing || (playerInput.MoveX || playerInput.MoveY)) && subCharacterController.RunCheck())
        {
            stateMachine.SwitchState(typeof(SubCharacterState_Run));
        }
        //else if((subCharacterController.Moveing || (playerInput.MoveX || playerInput.MoveY)))
        //{
        //    stateMachine.SwitchState(typeof(SubCharacterState_Walk));
        //}
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
        subCharacterController.Following(runSpeed);
    }
}
