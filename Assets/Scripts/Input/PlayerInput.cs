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
/// 玩家輸入類 負責接收輸出按鍵輸入
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

    #region 輸入相關變數
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

        //遊戲狀態事件監聽註冊
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;

        //玩家行為事件監聽註冊
        GameManager.Instance.onNonePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInteractivePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onTalkingPlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;
        GameManager.Instance.onInCutScenePlayerBehaviourStateChanged += OnPlayerBehaviourStateChanged;


    }
    private void Start()
    {
        //HP歸0事件監聽
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
        // 檢查連接的手把
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            return gamepad;
        }

        // 如果沒有連接的手把，預設使用滑鼠
        return Mouse.current;
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
    private void HpZeroEvent()
    {
        isEnable = false;
    }
    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        isEnable = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
    /// <summary>
    /// 在特定玩家行為狀態下啟用
    /// </summary>
    private void OnPlayerBehaviourStateChanged(PlayerBehaviourState playerBehaviourState)
    {
        isEnable = playerBehaviourState == PlayerBehaviourState.None;
    }
}
