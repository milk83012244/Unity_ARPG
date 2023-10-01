using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Run", fileName = "PlayerState_Run")]
public class PlayerState_Run : PlayerState
{
    [SerializeField] Dictionary<string, float> runSpeed = new Dictionary<string, float>();
    [HideInInspector] public float currentRunSpeed;
    //[SerializeField] float acceration = 5f; //移動時的加速度

    public override void Enter()
    {
        if (input.AxisX < 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Run");
            
            //animator.Play(player.currentControlCharacterNames + "_SL_Run");
        }
        else if (input.AxisX > 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Run");
        }
        else if (input.AxisY > 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_B_Run");
        }
        else if (input.AxisY < 0)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_F_Run");
        }
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Niru":
                currentRunSpeed = runSpeed["Niru"];
                break;
            case "Mo":
                currentRunSpeed = runSpeed["Mo"];
                break;
            case "Lia":
                currentRunSpeed = runSpeed["Lia"];
                break;
        }
        //currentSpeedx = Mathf.MoveTowards(currentSpeedx, walkSpeed, acceration * Time.deltaTime);
        //currentSpeedy = Mathf.MoveTowards(currentSpeedy, walkSpeed, acceration * Time.deltaTime);
    }

    public override void LogicUpdate()
    {
        InPause();

        DamageState();

        if (!input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
        if (input.MoveX && !input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        else if (!input.MoveX && input.MoveY)
        {
            stateMachine.SwitchState(typeof(PlayerState_Run));
        }
        if (!input.PressRun)
        {
            stateMachine.SwitchState(typeof(PlayerState_Walk));
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
        player.Move(currentRunSpeed);
    }
}
