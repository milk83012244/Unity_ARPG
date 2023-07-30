using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Dodge", fileName = "PlayerState_Dodge")]
public class PlayerState_Dodge : PlayerState
{
    [SerializeField] Dictionary<string, float> dodgeSpeed = new Dictionary<string, float>();
    [HideInInspector] public float currentDodgeSpeed;

    public static bool isDodge;
    public override void Enter()
    {
        
        //如果角色沒有此動作就返回待機
        //switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        //{
        //    case "Lia":
        //        stateMachine.SwitchState(typeof(PlayerState_Idle));
        //        return;
        //        //break;
        //}

        base.Enter();
        isDodge = true;
        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Mo":
                playerEffectSpawner.DodgeSmokeTrigger.Invoke();
                //return;
                break;
        }

        if (input.currentDirection == 1)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Dodge");
        }
        else if (input.currentDirection == 3)
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SR_Dodge");
        }
        else
        {
            animator.Play(playerCharacterSwitch.currentControlCharacterNamesSB.ToString() + "_SL_Dodge");
        }

        switch (playerCharacterSwitch.currentControlCharacterNamesSB.ToString())
        {
            case "Niru":
                currentDodgeSpeed = dodgeSpeed["Niru"];
                break;
            case "Mo":
                currentDodgeSpeed = dodgeSpeed["Mo"];
                break;
            case "Lia":
                currentDodgeSpeed = dodgeSpeed["Lia"];
                break;
        }
        base.SwitchCharacterState(false);
        DodgeMove();
    }
    public override void Exit()
    {
        isDodge = false;
        playerCooldownController.DodgeCooldownTrigger.Invoke();
    }
    public override void LogicUpdate()
    {
        if (IsAnimationFinished)
        {
            base.SwitchCharacterState(true);

            stateMachine.SwitchState(typeof(PlayerState_Idle));
        }
    }
    public override void PhysicUpdate()
    {
        //player.Move(dodgeSpeed);
    }

    void DodgeMove()
    {
        Vector2 dodgeDir;
        if (input.currentDirection == 1)
        {
            dodgeDir = Vector2.left;
            player.DodgeMove(dodgeDir, currentDodgeSpeed);
        }
        else if (input.currentDirection == 2)
        {
            dodgeDir = Vector2.down;
            player.DodgeMove(dodgeDir, currentDodgeSpeed);
        }
        else if (input.currentDirection == 3)
        {
            dodgeDir = Vector2.right;
            player.DodgeMove(dodgeDir, currentDodgeSpeed);
        }
        else if (input.currentDirection == 4)
        {
            dodgeDir = Vector2.up;
            player.DodgeMove(dodgeDir, currentDodgeSpeed);
        }
        if (input.AxisX >0 && input.AxisY > 0) //右上
        {
            dodgeDir = new Vector2(1, 1);
            player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
        }
        else if (input.AxisX < 0 && input.AxisY < 0) //左下
        {
            dodgeDir = new Vector2(-1, -1);
            player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
        }
        else if (input.AxisX > 0 && input.AxisY < 0) //右下
        {
            dodgeDir = new Vector2(1, -1);
            player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
        }
        else if (input.AxisX < 0 && input.AxisY >0) //左上
        {
            dodgeDir = new Vector2(-1, 1);
            player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
        }
    }
}
