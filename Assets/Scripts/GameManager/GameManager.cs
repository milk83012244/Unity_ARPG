using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // �w�qGameManager����ҡA�H�K�b��L�}�����X�ݥ�
    public static GameManager instance;

    // �w�q�C�������A�A�Ҧp�C���i�椤�B�Ȱ��B������
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    public bool isBattleMode = false;

    public GameState CurrentGameState;

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
    }
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("�S��UIManager���");
            return instance;
        }
        else
        {
            return instance;
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
        CurrentGameState = GameState.Playing;
    }
}
