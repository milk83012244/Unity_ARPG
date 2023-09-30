using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPÂk0ª¬ºA
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Down", fileName = "PlayerState_Down")]
public class PlayerState_Down : PlayerState
{
    public static bool isDown;
    public override void Enter()
    {
        base.Enter();
        isDown = true;

        if (input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Down");
        }
        else if (input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Down");
        }
        else
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Down");
        }
    }
    public override void Exit()
    {
        isDown = false;
    }
    public override void LogicUpdate()
    {
        player.SetVelocityX(currentSpeedx);
        player.SetVelocityY(currentSpeedy);
        player.SetVelocityXY(0, 0);
    }
    public override void PhysicUpdate()
    {

    }
}
