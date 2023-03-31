using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 定義GameManager的實例，以便在其他腳本中訪問它
    public static GameManager instance;

    // 定義遊戲的狀態，例如遊戲進行中、暫停、結束等
    public enum GameState
    {
        Normel,
        Battle,
        Paused,
        GameOver
    }

    public bool isBattleMode = false;

    public GameState CurrentGameState;

    private void Awake()
    {
        SwitchNormelMode();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有GameManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }
    public void SwitchNormelMode()
    {
        CurrentGameState = GameState.Normel;
    }
    public void SwitchBattleMode()
    {
        CurrentGameState = GameState.Battle;
    }
    public int GetCurrentState()
    {
        return (int)CurrentGameState;
    }
    public void PauseGame()
    {
        // 暫停遊戲代碼
        // ...

        // 設置遊戲狀態為暫停
        CurrentGameState = GameState.Paused;
    }
    public void ResumeGame()
    {
        // 繼續遊戲代碼
        // ...

        // 設置遊戲狀態為遊戲進行中
        CurrentGameState = GameState.Normel;
    }
}
