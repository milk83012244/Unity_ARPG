using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Walk", fileName = "PlayerState_Walk")]
public class PlayerState_Walk : PlayerState
{
    [SerializeField] Dictionary<string, float> walkSpeed = new Dictionary<string, float>();
    [HideInInspector] public float currentWalkSpeed;
    //[SerializeField] float acceleration = 5f; //移動時的加速度

    public override void Enter()
    {
        if (input.AxisX < 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Walk");
        }
        else if (input.AxisX > 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Walk");
        }
        else if (input.AxisY > 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Walk");
        }
        else if (input.AxisY < 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Walk");
        }

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Niru":
                currentWalkSpeed = walkSpeed["Niru"];
                break;
            case "Mo":
                currentWalkSpeed = walkSpeed["Mo"];
                break;
            case "Lia":
                currentWalkSpeed = walkSpeed["Lia"];
                break;
        }
        currentSpeedx = Mathf.MoveTowards(currentSpeedx, currentWalkSpeed, Time.deltaTime);
        currentSpeedy = Mathf.MoveTowards(currentSpeedy, currentWalkSpeed, Time.deltaTime);
    }
    public override void LogicUpdate()
    {
        if (!input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }

        if (input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Walk));
        }
        else if (!input.MoveX && input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Walk));
        }
        if (input.PressRun)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        if (input.PressDodge)
        {
            stateMachine.SwitchState(typeof(PlayerState_Dodge));
        }

        //攻擊
        if (input.PressAttack)
        {
            stateMachine.SwitchState(typeof(PlayerState_Attack));
        }
        base.UseSkill();

        if (input.PressGuard)
        {
            stateMachine.SwitchState(typeof(PlayerState_Guard));
        }
    }
    public override void PhysicUpdate()
    {
        if (input.MoveX)
        {
            player.Move(currentSpeedx);
        }
        else if (input.MoveY)
        {
            player.Move(currentSpeedy);
        }
        if (input.MoveX && input.MoveY)
        {
            player.MoveXY(currentSpeedx, currentSpeedy);
        }
        // player.Move(currentSpeedY);
        //player.SetVelocityY(runSpeed);
    }
}
