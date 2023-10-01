using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Attacks/Attack", fileName = "PlayerState_Attack")]
public class PlayerState_Attack : PlayerState
{
    public static bool isAttack1;
    public override void Enter()
    {
        //各角色獨自有的狀態
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Lia":
                LiaElementSwitch.canSwitch = false;
                break;
        }

        isAttack1 = true;
        base.Enter();
        if (characterStats.characterData[characterStats.currentCharacterID].attackType == AttackType.Melee)
        {
            if (input.PressAttack && input.currentDirection == 1)
            {
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Attack1");
            }
            else if (input.PressAttack && input.currentDirection == 3)
            {
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Attack1");
            }
            else if (input.PressAttack && input.currentDirection == 2)
            {
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Attack1");
            }
            else if (input.PressAttack && input.currentDirection == 4)
            {
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Attack1");
            }
        }
        else if (characterStats.characterData[characterStats.currentCharacterID].attackType == AttackType.RangedAttack)
        {
            AimRotate aimRotate = player.rangedAimObject.GetComponent<AimRotate>();
            if (input.PressAttack && aimRotate.currentDirection == 1)
            {
                input.currentDirection = 1;
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Attack1");
            }
            else if (input.PressAttack && aimRotate.currentDirection == 3)
            {
                input.currentDirection = 3;
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Attack1");
            }
            else if (input.PressAttack && aimRotate.currentDirection == 2)
            {
                input.currentDirection = 2;
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Attack1");
            }
            else if (input.PressAttack && aimRotate.currentDirection == 4)
            {
                input.currentDirection = 4;
                animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Attack1");
            }
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

        isAttack1 = false;
    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);

        DamageState();

        if (CurrentStateTime >= 0.7f)
        {
            base.SwitchCharacterState(true);
            if (input.PressAttack)
            {
                stateMachine.SwitchState(typeof(PlayerState_Attack2));
            }
            base.UseSkill();
            if (input.PressGuard)
            {
                stateMachine.SwitchState(typeof(PlayerState_Guard));
            }

            if (input.PressDodge)
            {
                stateMachine.SwitchState(typeof(PlayerState_Dodge));
            }
        }
        if (IsAnimationFinished)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
}
