using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Idle", fileName = "PlayerState_Idle")]
public class PlayerState_Idle : PlayerState
{
    [SerializeField] float deacceration = 5f; //停止移動時減速度

    public override void Enter()
    {
        if (isInitial)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_FrontIdle");
            base.isInitial = false;
        }
        else
        {
            if (GameManager.GetInstance().GetCurrentState() == (int)GameManager.GameState.Normel) //一般待機
            {
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_FrontIdle");
            }
            else if (GameManager.GetInstance().GetCurrentState() == (int)GameManager.GameState.Battle) //戰鬥待機
            {
                if (input.currentDirection == 1)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_BattleIdle");
                }
                else if (input.currentDirection == 3)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_BattleIdle");
                }
                else
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_BattleIdle");
                }
            }
        }

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
        if (input.PressSkill1)
        {
            stateMachine.SwitchState(typeof(PlayerState_Skill1));
        }
        if (input.PressSkill2)
        {
            stateMachine.SwitchState(typeof(PlayerState_Skill2));
        }
    }
    public override void PhysicUpdate()
    {
        player.SetVelocityX(this.currentSpeedx);
        player.SetVelocityY(this.currentSpeedy);
        player.SetVelocityXY(0, 0);
    }
}
