using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Skill2", fileName = "PlayerState_Skill2")]
public class PlayerState_Skill2 : PlayerState
{
    [SerializeField] float MoSkill2Speed = 5f;

    public static bool isSkill2Enter;
    public override void Enter()
    {
        //各角色獨自有的狀態
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                LiaElementSwitch.canSwitch = false;
                break;
        }

        base.Enter();

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                isSkill2Enter = true;

                if (input.PressSkill2 && input.currentDirection == 1)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Skill2");
                }
                else if (input.PressSkill2 && input.currentDirection == 3)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Skill2");
                }
                else if (input.PressSkill2 && input.currentDirection == 2)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Skill2");
                }
                else if (input.PressSkill2 && input.currentDirection == 4)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Skill2");
                }

                MoSkill2Move();
                break;
            case "Lia":
                if (input.PressSkill2)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Skill2");
                }
                break;
        }

        base.SwitchCharacterState(false);
        //技能CD
        playerCooldownController.Skill2CooldownTrigger.Invoke(playerCharacterSwitch.currentControlCharacterNamesSB.ToString());
    }
    public override void Exit()
    {
        //各角色獨自有的狀態
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                LiaElementSwitch.canSwitch = true;
                break;
        }

        MoveEnd();
        isSkill2Enter = false;
    }
    public override void LogicUpdate()
    {
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                break;
            case "Lia":
                player.SetVelocityX(currentSpeedx);
                player.SetVelocityY(currentSpeedy);
                player.SetVelocityXY(0, 0);
                break;
        }

        if (IsAnimationFinished)
        {
            base.SwitchCharacterState(true);

            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }

    void MoSkill2Move()
    {
        Vector2 MoveDir;
        if (input.currentDirection == 1)
        {
            MoveDir = Vector2.left;
            player.MoSkill2Move(MoveDir, MoSkill2Speed);
        }
        else if (input.currentDirection == 2)
        {
            MoveDir = Vector2.down;
            player.MoSkill2Move(MoveDir, MoSkill2Speed);
        }
        else if (input.currentDirection == 3)
        {
            MoveDir = Vector2.right;
            player.MoSkill2Move(MoveDir, MoSkill2Speed);
        }
        else if (input.currentDirection == 4)
        {
            MoveDir = Vector2.up;
            player.MoSkill2Move(MoveDir, MoSkill2Speed);
        }
        if (input.AxisX > 0 && input.AxisY > 0) //右上
        {
            MoveDir = new Vector2(1, 1);
            player.MoSkill2MoveXY(MoveDir, MoSkill2Speed, MoSkill2Speed);
        }
        else if (input.AxisX < 0 && input.AxisY < 0) //左下
        {
            MoveDir = new Vector2(-1, -1);
            player.MoSkill2MoveXY(MoveDir, MoSkill2Speed, MoSkill2Speed);
        }
        else if (input.AxisX > 0 && input.AxisY < 0) //右下
        {
            MoveDir = new Vector2(1, -1);
            player.MoSkill2MoveXY(MoveDir, MoSkill2Speed, MoSkill2Speed);
        }
        else if (input.AxisX < 0 && input.AxisY > 0) //左上
        {
            MoveDir = new Vector2(-1, 1);
            player.MoSkill2MoveXY(MoveDir, MoSkill2Speed, MoSkill2Speed);
        }
    }
    void MoveEnd()
    {
        player.SetVelocityX(this.currentSpeedx);
        player.SetVelocityY(this.currentSpeedy);
        player.SetVelocityXY(0, 0);
    }
}
