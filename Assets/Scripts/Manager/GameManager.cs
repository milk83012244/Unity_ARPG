using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum GameState //遊戲狀態
{
    Normal,
    Battle,
    Paused,
    GameOver,
    InMenu //只在主選單使用
}
public enum PlayerBehaviourState //玩家行為邏輯狀態
{
    None,
    Interactive,
    Talking,
    InCutScene,
}
public enum BattleFieldState //戰鬥區域類型
{
    None,
    NormalBattle,
    BossBattle,
    EventBattle,
}
public class GameManager : SerializedMonoBehaviour,IDataPersistence
{
    // 定義GameManager的實例，以便在其他腳本中訪問它
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameState CurrentGameState { get; private set; }
    public PlayerBehaviourState CurrentPlayerBehaviourState { get; private set; }
    public BattleFieldState CurrentBattleFieldState { get; private set; }

    //遊戲狀態切換委派
    public delegate void GameStateHander(GameState newGameState);
    //玩家狀態切換委派
    public delegate void PlayerBehaviourStateHander(PlayerBehaviourState newGameState);
    //場地狀態切換委派
    public delegate void BattleFieldStateHander(BattleFieldState newGameState);

    //遊戲狀態切換事件監聽
    public event GameStateHander onNormalGameStateChanged;
    public event GameStateHander onBattleGameStateChanged;
    public event GameStateHander onPasueGameStateChanged;
    public event GameStateHander onGameOverGameStateChanged;
    //玩家狀態切換事件監聽
    public event PlayerBehaviourStateHander onNonePlayerBehaviourStateChanged;
    public event PlayerBehaviourStateHander onInteractivePlayerBehaviourStateChanged;
    public event PlayerBehaviourStateHander onTalkingPlayerBehaviourStateChanged;
    public event PlayerBehaviourStateHander onInCutScenePlayerBehaviourStateChanged;
    //戰鬥場地狀態切換事件監聽
    public event BattleFieldStateHander onNoneFieldStateChanged;
    public event BattleFieldStateHander onNormalBattleFieldStateChanged;
    public event BattleFieldStateHander onBossBattleFieldStateChanged;
    public event BattleFieldStateHander onEventFieldStateChanged;

    public DateTime saveTime;
    private float playTime;
    private float loadedPlayTime;

    public bool inMenu;
    public bool isNewGame; //Demo用檢測是否是新遊戲

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        SetState(GameState.Normal);
        SetPlayerBehaviourState(PlayerBehaviourState.None);

        if (inMenu)
        {
            SetState(GameState.InMenu);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StartCoroutine(TrackPlayTime());
    }

    public void LoadData(GameData gameData)
    {
        if (gameData != null)
        {
            loadedPlayTime = gameData.playTime;
        }
        else
        {
            loadedPlayTime = 0;
        }
        playTime = loadedPlayTime;
    }

    public void SaveData(GameData gameData)
    {
        this.saveTime = new DateTime();
        this.saveTime = DateTime.Now;
        gameData.saveTime = this.saveTime.ToString(); //保存時間
        gameData.playTime = playTime; //保存遊玩時間
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState)
            return;
        CurrentGameState = newGameState;
        switch (newGameState)
        {
            case GameState.Normal:
                onNormalGameStateChanged?.Invoke(newGameState);
                break;
            case GameState.Battle:
                onBattleGameStateChanged?.Invoke(newGameState);
                break;
            case GameState.Paused:
                onPasueGameStateChanged?.Invoke(newGameState);
                break;
            case GameState.GameOver:
                onGameOverGameStateChanged?.Invoke(newGameState);
                break;
        }
    }
    public void SetPlayerBehaviourState(PlayerBehaviourState playerState)
    {
        if (playerState == CurrentPlayerBehaviourState)
            return;
        CurrentPlayerBehaviourState = playerState;
        switch (playerState)
        {
            case PlayerBehaviourState.None:
                onNonePlayerBehaviourStateChanged?.Invoke(playerState);
                break;
            case PlayerBehaviourState.Interactive:
                onInteractivePlayerBehaviourStateChanged?.Invoke(playerState);
                break;
            case PlayerBehaviourState.Talking:
                onTalkingPlayerBehaviourStateChanged?.Invoke(playerState);
                break;
            case PlayerBehaviourState.InCutScene:
                onInCutScenePlayerBehaviourStateChanged?.Invoke(playerState);
                break;
        }
    }
    public void SetBattleFieldState(BattleFieldState battleFieldState)
    {
        switch (battleFieldState)
        {
            case BattleFieldState.None:
                onNoneFieldStateChanged?.Invoke(battleFieldState);
                break;
            case BattleFieldState.NormalBattle:
                onNormalBattleFieldStateChanged?.Invoke(battleFieldState);
                break;
            case BattleFieldState.BossBattle:
                onBossBattleFieldStateChanged?.Invoke(battleFieldState);
                break;
            case BattleFieldState.EventBattle:
                onEventFieldStateChanged?.Invoke(battleFieldState);
                break;
        }
    }
    public int GetCurrentState()
    {
        return (int)CurrentGameState;
    }
    public PlayerBehaviourState GetCurrentPlayerBehaviourState()
    {
        return CurrentPlayerBehaviourState;
    }
    public BattleFieldState GetBattleFieldState()
    {
        return CurrentBattleFieldState;
    }
    /// <summary>
    /// 獲得當前控制角色號碼
    /// </summary>
    public int GetCurrentBattleCharacter()
    {
        switch (PlayerCharacterSwitch.battleCurrentCharacterNumber)
        {
            case BattleCurrentCharacterNumber.First:
                return 1;
            case BattleCurrentCharacterNumber.Second:
                return 2;
            case BattleCurrentCharacterNumber.Third:
                return 3;
            default:
                return 0;
        }
    }
    public void MenuStartGame()
    {
        inMenu = false;
        SetState(GameState.Normal);
    }
    /// <summary>
    /// 計算遊玩時間
    /// </summary>
    private IEnumerator TrackPlayTime()
    {
        while (true)
        {
            playTime += Time.deltaTime;
            yield return null;
        }
    }
}
