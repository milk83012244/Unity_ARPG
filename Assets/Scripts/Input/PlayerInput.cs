using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    public int currentDirection;

    #region 輸入相關變數
    public float AxisX => Axes.x;
    public float AxisY => Axes.y;
    Vector2 Axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();

    public bool MoveX => AxisX != 0f;
    public bool MoveY => AxisY != 0f;

    public bool PressRun => playerInputActions.Gameplay.Run.IsPressed() == true;
    public bool PressAttack => playerInputActions.Gameplay.Attack.IsPressed() && GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle;
    public bool ChangeCharacter1 => playerInputActions.Gameplay.SwitchCharacter.IsPressed() && GameManager.GetInstance().isBattleMode == true;
    //public bool Attack => playerInputActions.Gameplay.Attack.WasPressedThisFrame();
    #endregion

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        currentDirection = 0;
    }
    private void Update()
    {
        currentDirection = PlayerCurrentDir();

    }
    public void EnableGamePlayInputs()
    {
        playerInputActions.Gameplay.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
    }
    /// <summary>
    /// 玩家當前轉向(其他需要轉向的動作用)
    /// </summary>
    private int PlayerCurrentDir()
    {
        if (AxisX < 0)
        {
            return 1; //左
        }
        else if (AxisX > 0)
        {
            return 3; //右
        }
        else if (AxisY > 0)
        {
            return 4; //上
        }
        else if (AxisY < 0)
        {
            return 2; //下
        }
        else
        {
            return currentDirection;
        }
    }
}
