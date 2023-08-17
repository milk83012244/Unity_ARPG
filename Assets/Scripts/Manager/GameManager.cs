using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public enum GameState
{
    Normal,
    Battle,
    Paused,
    GameOver
}
public enum PlayerBehaviourState //���a�欰�޿説�A
{
    None,
    Interactive,
    Talking,
}
public class GameManager : SerializedMonoBehaviour,IDataPersistence
{
    // �w�qGameManager����ҡA�H�K�b��L�}�����X�ݥ�
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

    //�C�����A�����e��
    public delegate void GameStateHander(GameState newGameState);
    //���a���A�����e��
    public delegate void PlayerBehaviourStateHander(PlayerBehaviourState newGameState);

    //�C�����A�����ƥ��ť
    public event GameStateHander onNormalGameStateChanged;
    public event GameStateHander onBattleGameStateChanged;
    public event GameStateHander onPasueGameStateChanged;
    public event GameStateHander onGameOverGameStateChanged;
    //���a���A�����ƥ��ť
    public event PlayerBehaviourStateHander onNonePlayerBehaviourStateChanged;
    public event PlayerBehaviourStateHander onInteractivePlayerBehaviourStateChanged;
    public event PlayerBehaviourStateHander onTalkingPlayerBehaviourStateChanged;

    public DateTime saveTime;
    private float playTime;
    private float loadedPlayTime;

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
        gameData.saveTime = this.saveTime.ToString();
        gameData.playTime = playTime;
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
    /// <summary>
    /// ��o��e����⸹�X
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
    public void PauseGame()
    {
        // �Ȱ��C���N�X
        // ...

        // �]�m�C�����A���Ȱ�
        CurrentGameState = GameState.Paused;
    }
    public void ResumeGame()
    {
        // �~��C���N�X
        // ...

        // �]�m�C�����A���C���i�椤
        CurrentGameState = GameState.Normal;
    }
    /// <summary>
    /// �p��C���ɶ�
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
