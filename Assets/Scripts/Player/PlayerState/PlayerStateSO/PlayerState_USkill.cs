using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/USkill", fileName = "PlayerState_USkill")]
public class PlayerState_USkill : PlayerState
{
    public static bool isUSkillEnter;
    public override void Enter()
    {
        isUSkillEnter = true;
        base.Enter();

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                if (input.PressUSkill)
                {
                    animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_USkill");
                }
                break;
        }

        base.SwitchCharacterState(false);
        //ßﬁØ‡CD
        playerCooldownController.USkillCooldownTrigger.Invoke(playerCharacterSwitch.currentControlCharacterNamesSB.ToString());
    }

    public override void Exit()
    {
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
