using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    public int currentDirection;

    #region ��J�����ܼ�
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
    /// ���a��e��V(��L�ݭn��V���ʧ@��)
    /// </summary>
    private int PlayerCurrentDir()
    {
        if (AxisX < 0)
        {
            return 1; //��
        }
        else if (AxisX > 0)
        {
            return 3; //�k
        }
        else if (AxisY > 0)
        {
            return 4; //�W
        }
        else if (AxisY < 0)
        {
            return 2; //�U
        }
        else
        {
            return currentDirection;
        }
    }
}
