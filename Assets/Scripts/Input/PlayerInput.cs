using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ���a��J�� �t�d������X�����J
/// </summary>
public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;

    public int currentDirection;

    public bool canDodge;
    public bool canSkill1;

    #region ��J�����ܼ�
    public float AxisX => Axes.x;

    public float AxisY => Axes.y;

    Vector2 Axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();

    public bool MoveX => AxisX != 0f;
    public bool MoveY => AxisY != 0f;

    public bool PressRun => playerInputActions.Gameplay.Run.IsPressed() == true ;
    public bool PressDodge => playerInputActions.Gameplay.Dodge.IsPressed() == true && canDodge && GameManager.GetInstance().GetCurrentState() == (int)GameManager.GameState.Battle;
    public bool PressAttack => playerInputActions.Gameplay.Attack.IsPressed() && GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle;
    public bool PressSkill1 => playerInputActions.Gameplay.Skill1.WasPressedThisFrame() && canSkill1&& GameManager.GetInstance().CurrentGameState == GameManager.GameState.Battle;
    public bool ChangeCharacter1 => playerInputActions.Gameplay.SwitchCharacter.IsPressed() && GameManager.GetInstance().GetCurrentState() == (int)GameManager.GameState.Battle;
    //public bool Attack => playerInputActions.Gameplay.Attack.WasPressedThisFrame();
    #endregion

    private void Awake()
    {
        canDodge = true;
        canSkill1 = true;
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
    public void DesableGamePlayInputs()
    {
        playerInputActions.Gameplay.Disable();
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
