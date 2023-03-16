using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    #region 輸入相關變數
    public float AxisX => Axes.x;
    public float AxisY => Axes.y;
    Vector2 Axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();

    public bool MoveX => AxisX != 0f;
    public bool MoveY => AxisY != 0f;

    public bool PressRun => playerInputActions.Gameplay.Run.IsPressed() == true;
    public bool PressAttack => playerInputActions.Gameplay.Attack.IsPressed() && GameManager.GetInstance().isBattleMode == true;
    public bool ChangeCharacter1 => playerInputActions.Gameplay.SwitchCharacter.IsPressed() && GameManager.GetInstance().isBattleMode == true;
    //public bool Attack => playerInputActions.Gameplay.Attack.WasPressedThisFrame();
    #endregion

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
    }
    public void EnableGamePlayInputs()
    {
        playerInputActions.Gameplay.Enable();
        //Cursor.lockState = CursorLockMode.Locked;
    }
}
