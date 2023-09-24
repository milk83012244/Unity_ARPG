using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public enum ControlMode
{
    KeyboardMouse,
    Gamepad
}
/// <summary>
/// ���a��J�� �t�d������X�����J
/// </summary>
public class PlayerInput : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    PlayerCharacterStats characterStats;
    PlayerController playerController;

    public int currentDirection;
    public LiaUnlockDataSO liaUnlockData;

    [HideInInspector] public List<bool> canDodge = new List<bool>();
    [HideInInspector] public List<bool> canSkill1 = new List<bool>();
    [HideInInspector] public List<bool> canSkill2 = new List<bool>();
    [HideInInspector] public List<bool> canUSkill = new List<bool>();

    [HideInInspector] public List<bool> canSwitchFunctionkey = new List<bool>();

    [HideInInspector] public List<bool> canCharacterSwitch = new List<bool>();

    private bool isEnable = true;
    private bool isUsingFunctionButton = false;
    private bool isUsingGamepad = false;

    private ControlMode currentControlMode = ControlMode.KeyboardMouse;

    #region ��J�����ܼ�
    public float AxisX => Axes.x;

    public float AxisY => Axes.y;

    Vector2 Axes => playerInputActions.Gameplay.Axes.ReadValue<Vector2>();

    public Vector2 AimAxes => playerInputActions.Gameplay.AimAxes.ReadValue<Vector2>();

    public bool MoveX => AxisX != 0f && isEnable;
    public bool MoveY => AxisY != 0f && isEnable;

    public bool PressRun => playerInputActions.Gameplay.Run.IsPressed() == true && isEnable;
    public bool PressDodge => playerInputActions.Gameplay.Dodge.IsPressed() == true && canDodge[characterStats.currentCharacterID] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    public bool PressAttack => playerInputActions.Gameplay.Attack.IsPressed() && !EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.CurrentGameState == GameState.Battle;
    public bool PressGuard => playerInputActions.Gameplay.Guard.IsPressed() && GameManager.Instance.CurrentGameState == GameState.Battle;
    public bool PressSkill1 => playerInputActions.Gameplay.Skill1.WasPressedThisFrame() &&canSkill1[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill1;
    public bool PressSkill1Release => playerInputActions.Gameplay.Skill1.WasReleasedThisFrame() && canSkill1[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill1;
    public bool PressingSkill1 => playerInputActions.Gameplay.Skill1.IsPressed() && canSkill1[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill1;

    public bool PressSkill2 => playerInputActions.Gameplay.Skill2.WasPressedThisFrame() && canSkill2[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill2;
    public bool PressSkill2Release => playerInputActions.Gameplay.Skill2.WasReleasedThisFrame() && canSkill2[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill2;
    public bool PressingSkill2 => playerInputActions.Gameplay.Skill2.IsPressed() && canSkill2[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseSkill2;

    public bool PressUSkill => playerInputActions.Gameplay.USkill.WasPressedThisFrame() && canUSkill[characterStats.currentCharacterID] && GameManager.Instance.CurrentGameState == GameState.Battle && playerController.canUseUSkill;

    public bool CharacterSwitch1 => playerInputActions.Gameplay.SwitchCharacter1.WasPressedThisFrame()&& canCharacterSwitch[0] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle &&
        GameManager.Instance.GetCurrentBattleCharacter() != (int)BattleCurrentCharacterNumber.First && !isUsingFunctionButton;
    public bool CharacterSwitch2 => playerInputActions.Gameplay.SwitchCharacter2.WasPressedThisFrame() && canCharacterSwitch[1] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle &&
        GameManager.Instance.GetCurrentBattleCharacter() != (int) BattleCurrentCharacterNumber.Second && !isUsingFunctionButton;
    public bool CharacterSwitch3 => playerInputActions.Gameplay.SwitchCharacter3.WasPressedThisFrame() && canCharacterSwitch[2] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle &&
    GameManager.Instance.GetCurrentBattleCharacter() != (int)BattleCurrentCharacterNumber.Third && !isUsingFunctionButton;

    public bool FunctionkeyReady => playerInputActions.Gameplay.ReadyFunctionKey.IsPressed() && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    public bool SwitchFunctionkey1 => playerInputActions.Gameplay.SwitchFunctionkey1.WasPressedThisFrame() && canSwitchFunctionkey[0] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    public bool SwitchFunctionkey2 => playerInputActions.Gameplay.SwitchFunctionkey2.WasPressedThisFrame() && canSwitchFunctionkey[1] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    public bool SwitchFunctionkey3 => playerInputActions.Gameplay.SwitchFunctionkey3.WasPressedThisFrame() && canSwitchFunctionkey[2] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    public bool SwitchFunctionkey4 => playerInputActions.Gameplay.SwitchFunctionkey4.WasPressedThisFrame() && canSwitchFunctionkey[3] && GameManager.Instance.GetCurrentState() == (int)GameState.Battle;
    #endregion

    private void OnEnable()
    {
        playerInputActions.Gameplay.ReadyFunctionKey.performed += ctx => FunctionKeyActive();
        playerInputActions.Gameplay.ReadyFunctionKey.canceled += ctx => FunctionKeyDeactive();

        for (int i = 0; i < 6; i++)
        {
            canDodge.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canSkill1.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canSkill2.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canUSkill.Add(true);
        }
        for (int i = 0; i < 3; i++)
        {
            canCharacterSwitch.Add(true);
        }
        for (int i = 0; i < 4; i++)
        {
            canSwitchFunctionkey.Add(true);
        }
    }
    private void OnDestroy()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;

        GameManager.Instance.onNonePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInteractivePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onTalkingPlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInCutScenePlayerBehaviourStateChanged -= OnPlayerBehaviourStateChanged;

        characterStats.hpZeroEvent -= HpZeroEvent;
    }
    private void Awake()
    {
        characterStats = GetComponent<PlayerCharacterStats>();
        playerController = GetComponent<PlayerController>();

        for (int i = 0; i < 6; i++)
        {
            canDodge.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canSkill1.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canSkill2.Add(true);
        }
        for (int i = 0; i < 6; i++)
        {
            canUSkill.Add(true);
        }
        for (int i = 0; i < 3; i++)
        {
            canCharacterSwitch.Add(true);
        }
        for (int i = 0; i < 4; i++)
        {
            canSwitchFunctionkey.Add(true);
        }

        playerInputActions = new PlayerInputActions();
        currentDirection = 0;

        //�C�����A�ƥ��ť���U
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;

        //���a�欰�ƥ��ť���U
        GameManager.Instance.onNonePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInteractivePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onTalkingPlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInCutScenePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;


    }
    private void Start()
    {
        //HP�k0�ƥ��ť
        characterStats.hpZeroEvent += HpZeroEvent;

        GetDefaultInputDevice();
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
    public void FunctionKeyActive()
    {
        isUsingFunctionButton = true;
    }
    public void FunctionKeyDeactive()
    {
        isUsingFunctionButton = false;
    }

    private InputDevice GetDefaultInputDevice()
    {
        // �ˬd�s�������
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            return gamepad;
        }

        // �p�G�S���s�������A�w�]�ϥηƹ�
        return Mouse.current;
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
    private void HpZeroEvent()
    {
        isEnable = false;
    }
    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        isEnable = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
    /// <summary>
    /// �b�S�w���a�欰���A�U�ҥ�
    /// </summary>
    private void OnPlayerBehaviourStateChanged(PlayerBehaviourState playerBehaviourState)
    {
        isEnable = playerBehaviourState == PlayerBehaviourState.None;
    }
}
