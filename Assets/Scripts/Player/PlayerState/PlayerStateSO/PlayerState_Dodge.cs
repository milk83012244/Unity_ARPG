using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/StateMachine/PlayerState/Dodge", fileName = "PlayerState_Dodge")]
public class PlayerState_Dodge : PlayerState
{
    [SerializeField] Dictionary<string, float> dodgeSpeed = new Dictionary<string, float>();
    [SerializeField] Dictionary<string, float> dodgeDuration = new Dictionary<string, float>();
    [HideInInspector] public float currentDodgeSpeed;
    [HideInInspector] public float currentDodgeDuration;

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
                currentDodgeDuration = dodgeDuration["Niru"];
                break;
            case "Mo":
                currentDodgeSpeed = dodgeSpeed["Mo"];
                currentDodgeDuration = dodgeDuration["Mo"];
                break;
            case "Lia":
                currentDodgeSpeed = dodgeSpeed["Lia"];
                currentDodgeDuration = dodgeDuration["Lia"];
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
        if (input.currentDirection == 1 && input.AxisY == 0)
        {
            dodgeDir = Vector2.left;
            //player.DodgeMove(dodgeDir, currentDodgeSpeed);
            player.StartDodgeMoveCor(dodgeDir, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.currentDirection == 2 && input.AxisX == 0)
        {
            dodgeDir = Vector2.down;
            //player.DodgeMove(dodgeDir, currentDodgeSpeed);
            player.StartDodgeMoveCor(dodgeDir, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.currentDirection == 3 && input.AxisY == 0)
        {
            dodgeDir = Vector2.right;
            //player.DodgeMove(dodgeDir, currentDodgeSpeed);
            player.StartDodgeMoveCor(dodgeDir, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.currentDirection == 4 && input.AxisX == 0)
        {
            dodgeDir = Vector2.up;
            //player.DodgeMove(dodgeDir, currentDodgeSpeed);
            player.StartDodgeMoveCor(dodgeDir, currentDodgeSpeed, currentDodgeDuration);
        }
        if (input.AxisX >0 && input.AxisY > 0) //右上
        {
            dodgeDir = new Vector2(1, 1);
            //player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
            player.StartDodgeMoveXYCor(dodgeDir, currentDodgeSpeed, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.AxisX < 0 && input.AxisY < 0) //左下
        {
            dodgeDir = new Vector2(-1, -1);
            //player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
            player.StartDodgeMoveXYCor(dodgeDir, currentDodgeSpeed, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.AxisX > 0 && input.AxisY < 0) //右下
        {
            dodgeDir = new Vector2(1, -1);
            //player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
            player.StartDodgeMoveXYCor(dodgeDir, currentDodgeSpeed, currentDodgeSpeed, currentDodgeDuration);
        }
        else if (input.AxisX < 0 && input.AxisY >0) //左上
        {
            dodgeDir = new Vector2(-1, 1);
            //player.DodgeMoveXY(dodgeDir, currentDodgeSpeed, currentDodgeSpeed);
            player.StartDodgeMoveXYCor(dodgeDir, currentDodgeSpeed, currentDodgeSpeed, currentDodgeDuration);
        }
    }
}
