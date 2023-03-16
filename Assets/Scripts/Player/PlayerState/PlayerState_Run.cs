using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run", fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float acceration = 5f; //移動時的加速度

    public override void Enter()
    {
        if (input.AxisX < 0)
        {
            animator.Play(player.currentControlCharacterNames + "_SL_Run");
        }
        else if (input.AxisX > 0)
        {
            animator.Play(player.currentControlCharacterNames + "_SR_Run");
        }
        else if (input.AxisY > 0)
        {
            animator.Play(player.currentControlCharacterNames + "_B_Run");
        }
        else if (input.AxisY < 0)
        {
            animator.Play(player.currentControlCharacterNames + "_F_Run");
        }

        //currentSpeedx = Mathf.MoveTowards(currentSpeedx, walkSpeed, acceration * Time.deltaTime);
        //currentSpeedy = Mathf.MoveTowards(currentSpeedy, walkSpeed, acceration * Time.deltaTime);
    }

    public override void LogicUpdate()
    {
        if (!input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        if (input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        else if (!input.MoveX && input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        if (!input.PressRun)
        {
            stateMachine.SwitchState(typeof(PlayerState_Walk));
        }

        //攻擊
        if (input.PressAttack)
        {
            stateMachine.SwitchState(typeof(PlayerState_Attack));
        }
    }
    public override void PhysicUpdate()
    {
        player.Move(runSpeed);
        //if (input.MoveX)
        //{
        //    player.Move(currentSpeedx);
        //}
        //else if (input.MoveY)
        //{
        //    player.Move(currentSpeedy);
        //}
        //if (input.MoveX && input.MoveY)
        //{
        //    player.MoveXY(currentSpeedx, currentSpeedy);
        //}
    }
}
