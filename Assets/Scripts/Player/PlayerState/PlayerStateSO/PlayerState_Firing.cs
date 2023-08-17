using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能瞄準動作
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Firing", fileName = "PlayerState_Firing")]
public class PlayerState_Firing : PlayerState
{
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

        if (input.PressSkill1 && input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Skill1Firing");
        }
        else if (input.PressSkill1 && input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Skill1Firing");
        }
        else if (input.PressSkill1 && input.currentDirection == 2)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Skill1Firing");
        }
        else if (input.PressSkill1 && input.currentDirection == 4)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Skill1Firing");
        }

        player.SkillCursorObjectSetActive(true);

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                break;
            case "Lia":
                break;
        }
        base.SwitchCharacterState(false);
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

        player.SkillCursorObjectSetActive(false);
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

        if (input.PressSkill1Release)
        {
            base.SwitchCharacterState(true);

            stateMachine.SwitchState(typeof(PlayerState_Skill1));
        }
        //else if (skill2Firing && !input.PressingSkill2)
        //{
        //    stateMachine.SwitchState(typeof(PlayerState_Skill2));
        //}
    }
}
