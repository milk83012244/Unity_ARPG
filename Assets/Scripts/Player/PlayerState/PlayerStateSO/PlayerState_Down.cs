using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// HPÂk0ª¬ºA
/// </summary>
[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Down", fileName = "PlayerState_Down")]
public class PlayerState_Down : PlayerState
{
    public override void Enter()
    {

    }
    public override void Exit()
    {

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
