using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    [SerializeField] float deacceration = 5f; //停止移動時減速度
    public override void Enter()
    {
        animator.Play(player.currentControlCharacterNames + "_FrontIdle");

        currentSpeedx = player.MoveSpeedX;
        currentSpeedx = player.MoveSpeedY;
        //player.SetVelocityX(0);
        //player.SetVelocityY(0);
        //player.SetVelocityXY(0,0);
    }
    public override void LogicUpdate()
    {
        if (input.MoveX || input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Walk));
             if (input.PressRun)
            {
                stateMachine.SwitchState(typeof(PlayerState_Run));
            }
        }

        //減速度
        currentSpeedx = Mathf.MoveTowards(currentSpeedx, 0f, deacceration * Time.deltaTime);
        currentSpeedy = Mathf.MoveTowards(currentSpeedy, 0f, deacceration * Time.deltaTime);

        //攻擊
        if (input.PressAttack)
        {
            stateMachine.SwitchState(typeof(PlayerState_Attack));
        }
    }
    public override void PhysicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);
    }
}
