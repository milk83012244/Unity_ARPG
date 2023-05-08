using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Dodge", fileName = "PlayerState_Dodge")]
public class PlayerState_Dodge : PlayerState
{
    [SerializeField] float dodgeSpeed = 5f;

    public static bool isDodge;
    public override void Enter()
    {
        base.Enter();
        isDodge = true;
        playerEffectSpawner.DodgeSmokeTrigger.Invoke();

        if (input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Dodge");
        }
        else if (input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Dodge");
        }
        else
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Dodge");
        }
        DodgeMove();
    }
    public override void Exit()
    {
        isDodge = false;
        playerCooldownController.DodgeCooldownTrigger.Invoke();
    }
    public override void LogicUpdate()
    {
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
    public override void PhysicUpdate()
    {
        //player.Move(dodgeSpeed);
    }

    void DodgeMove()
    {
        Vector2 dodgeDir;
        if (input.currentDirection == 1)
        {
            dodgeDir = Vector2.left;
            player.DodgeMoveX(dodgeDir, dodgeSpeed);
        }
        else if (input.currentDirection == 2)
        {
            dodgeDir = Vector2.down;
            player.DodgeMoveY(dodgeDir, dodgeSpeed);
        }
        else if (input.currentDirection == 3)
        {
            dodgeDir = Vector2.right;
            player.DodgeMoveX(dodgeDir, dodgeSpeed);
        }
        else if (input.currentDirection == 4)
        {
            dodgeDir = Vector2.up;
            player.DodgeMoveY(dodgeDir, dodgeSpeed);
        }
        if (input.AxisX >0 && input.AxisY > 0) //右上
        {
            dodgeDir = new Vector2(1, 1);
            player.DodgeMoveXY(dodgeDir, dodgeSpeed, dodgeSpeed);
        }
        else if (input.AxisX < 0 && input.AxisY < 0) //左下
        {
            dodgeDir = new Vector2(-1, -1);
            player.DodgeMoveXY(dodgeDir, dodgeSpeed, dodgeSpeed);
        }
        else if (input.AxisX > 0 && input.AxisY < 0) //右下
        {
            dodgeDir = new Vector2(1, -1);
            player.DodgeMoveXY(dodgeDir, dodgeSpeed, dodgeSpeed);
        }
        else if (input.AxisX < 0 && input.AxisY >0) //左上
        {
            dodgeDir = new Vector2(-1, 1);
            player.DodgeMoveXY(dodgeDir, dodgeSpeed, dodgeSpeed);
        }
    }
}
