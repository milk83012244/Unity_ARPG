using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/USkill", fileName = "PlayerState_USkill")]
public class PlayerState_USkill : PlayerState
{
    public static bool isUSkillEnter;
    public override void Enter()
    {
        //�U����W�ۦ������A
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                LiaElementSwitch.canSwitch = false;
                break;
        }

        isUSkillEnter = true;
        base.Enter();

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                if (Live2DAnimationManager.Instance.MoUSkillAnimationEnd)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_USkill");
                    Live2DAnimationManager.Instance.MoUSkillAnimationEnd = false;
                }
                break;
        }

        base.SwitchCharacterState(false);
    }

    public override void Exit()
    {
        //�U����W�ۦ������A
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                LiaElementSwitch.canSwitch = true;
                break;
        }

        isUSkillEnter = false;
    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        if (IsAnimationFinished)
        {
            base.SwitchCharacterState(true);

            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
