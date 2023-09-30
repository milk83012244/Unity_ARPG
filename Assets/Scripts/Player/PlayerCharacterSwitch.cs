using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.Events;

/// <summary>
/// 玩家控制角色切換
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour, IDataPersistence
{
    public PartyDataSO partyData;
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //隊伍資料比對用
    //public List<string> characterNames = new List<string>();
    //public List<GameObject> characterObjs = new List<GameObject>();
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();
    //各角色碰撞器大小
    public Dictionary<string, Vector2> characterColliderSize = new Dictionary<string, Vector2>();
    public Dictionary<string, Vector2> characterColliderOffset = new Dictionary<string, Vector2>();

    public static BattleCurrentCharacterNumber battleCurrentCharacterNumber;

    public int currentUseCharacterID;

    public CharacterSwitchButtons characterSwitchButtons;
    public GameOverPanel gameOverPanel;
    public TipTextObject tipTextObject;

    private PlayerInput input;
    private PlayerState playerState;
    private PlayerController controller;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterStats characterStats;
    private PlayerCooldownController cooldownController;
    private PlayerLevelSystem levelSystem;
    private BoxCollider2D playerCollider2D;

    [HideInInspector] public PlayerSkillManager currentSkillManager;

    public delegate void PlayerSwitchHander(string characterName);

    public event PlayerSwitchHander onNormalToBattleMode;
    public event PlayerSwitchHander onBattleToNormalMode;
    public event PlayerSwitchHander onCharacterSwitch;
    public event PlayerSwitchHander onAwake;

    [HideInInspector]public UnityAction DownSwitchEnd;
    [HideInInspector] public UnityAction SwitchGameOverEvent;

    /// <summary>
    /// 當前控制角色
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();

    private void OnDestroy()
    {
        GameManager.Instance.onGameOverGameStateChanged -= OnGameOver;
        characterStats.hpZeroEvent -= StartNormalModeHPZeroCor;
    }
    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<PlayerController>();
        characterStats = GetComponent<PlayerCharacterStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        cooldownController = GetComponent<PlayerCooldownController>();
        levelSystem = GetComponent<PlayerLevelSystem>();
        playerCollider2D = GetComponent<BoxCollider2D>();

        StartSetCharacter("Niru");
    }
    private void Start()
    {
        GameManager.Instance.onGameOverGameStateChanged += OnGameOver;
        characterStats.hpZeroEvent += StartNormalModeHPZeroCor;
    }
    private void Update()
    {
        SwitchBattleCharacterInput();
    }
    public void LoadData(GameData gameData)
    {
        if (gameData.partyData != null)
        {
            partyData.currentParty = gameData.partyData;
        }
        else
        {
            gameData.partyData.Add(1, "");
            gameData.partyData.Add(2, "");
            gameData.partyData.Add(3, "");
            partyData.currentParty = gameData.partyData;
        }

        characterSwitchButtons.SetCharacterSlotIcon(partyData);
        onAwake?.Invoke(currentControlCharacterNamesSB.ToString());
    }

    public void SaveData(GameData gameData)
    {
        for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
        {
            gameData.partyData[i] = partyData.currentParty[i];
        }
        //foreach (KeyValuePair<int,string> pair in partyData.currentParty)
        //{
        //    gameData.partyData[pair.Key] = pair.Value;
        //}
        //gameData.partyData = this.partyData.currentParty;
    }

    /// <summary>
    ///  初始化角色
    /// </summary>
    public void StartSetCharacter(string characterName)
    {
        currentControlCharacter.Add(characterName, characterDic[characterName]);
        foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
        {
            currentControlCharacterNamesSB.Append(name.Key);
        }
        SetCharacterColliderSize();
    }

    /// <summary>
    /// 非戰鬥模式中切換角色
    /// </summary>
    public void SwitchCharacterInNormal()
    {

    }
    /// <summary>
    /// 戰鬥模式中切換角色
    /// </summary>
    public void SwitchCharacterInBattle(int number)
    {
        if (!characterSwitchButtons.canSwitch)
        {
            tipTextObject.startShowTextCor("當前無法切換角色");
            Debug.Log("當前無法切換角色");
            return;
        }

        if (number > partyData.currentParty.Count)
        {
            tipTextObject.startShowTextCor("隊伍編號" + number + "沒有角色");
            Debug.Log("隊伍編號" + number + "沒有角色");
            return;
        }
        if (!characterSwitchButtons.characterSwitchSlots[number].gameObject.activeSelf)
        {
            Debug.Log("欄位" + number + "沒有角色");
            return;
        }
        //檢查要更換的角色HP是否為0
        for (int j = 1; j < characterStats.characterData.Count; j++)
        {
            if (characterStats.characterData[j].characterName == partyData.currentParty[number])
            {
                if (characterStats.characterData[j].currentHealth <= 0)
                {
                    tipTextObject.startShowTextCor("欄位" + number + "角色HP為0無法戰鬥");
                    Debug.Log("欄位" + number + "角色HP為0無法戰鬥");
                    return;
                }
            }
        }

        string characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        characterDic[characterName].SetActive(false);

        battleCurrentCharacterNumber = (BattleCurrentCharacterNumber)number;
        characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]);

        characterStats.SetCurrentCharacterID(characterName);
        //currentControlCharacter[characterName].SetActive(true);
        characterDic[characterName].SetActive(true);

        cooldownController.CharacterSwitchCooldownTrigger.Invoke(characterName); //角色切換CD計時

        //角色按鈕狀態
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);

        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
        characterStats.ResetData();
        controller.SetCharacterStats(characterStats);

        //獲取當前控制角色名稱
        foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
        {
            if (currentControlCharacterNamesSB != null)
            {
                currentControlCharacterNamesSB.Clear();
            }
            currentControlCharacterNamesSB.Append(name.Key);
            // currentControlCharacterNames = name.Key;
        }
        //角色切換時啟動訂閱的事件
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());

        levelSystem.SetLevelSystemData();

        SetCharacterColliderSize();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
    }

    /// <summary>
    /// 從戰鬥模式切換回主角
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (currentControlCharacter.Count > 0)
        {
            currentControlCharacter.Clear();
        }

        if (currentControlCharacter.Count == 0)
        {
            foreach (var characterName in characterDic)
            {
                if (characterName.Key == "Niru") //關閉其他角色物件
                {
                    currentControlCharacter.Add(characterName.Key, characterDic[characterName.Key]);
                    //currentControlCharacter[characterName.Key].SetActive(true);
                    characterDic[characterName.Key].SetActive(true);
                }
                else
                {
                    characterDic[characterName.Key].SetActive(false);
                }
            }
            //獲取當前控制角色名稱
            foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
            {
                if (currentControlCharacterNamesSB != null)
                {
                    currentControlCharacterNamesSB.Clear();
                }
                currentControlCharacterNamesSB.Append(name.Key);
            }
        }
        characterStats.currentCharacterID = 0;
        controller.SetCharacterStats(characterStats);
        //角色切換時啟動訂閱的事件
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());
        //回一般模式啟動訂閱的事件
        onBattleToNormalMode?.Invoke(currentControlCharacterNamesSB.ToString());
        characterSwitchButtons.SetCurrnetUseCharacter("");

        levelSystem.SetLevelSystemData();

        SetCharacterColliderSize();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// 一般模式切換到戰鬥模式切換戰鬥角色
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);//之後可以改成一般模式當前使用角色

        //battleCurrentCharacterNumber = BattleCurrentCharacterNumber.First; //從第一個角色開始切換

        //遍歷目前隊伍列表(找到隊伍角色名)
        for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
        {
            if (!partyData.currentParty[i].IsNullOrWhitespace())
            {
                //檢查要更換的角色HP是否為0
                for (int j = 1; j < characterStats.characterData.Count; j++)
                {
                    if (characterStats.characterData[j].characterName == partyData.currentParty[i])
                    {
                        if (characterStats.characterData[j].currentHealth <= 0)
                        {
                            continue;
                        }
                        else
                        {
                            battleCurrentCharacterNumber = (BattleCurrentCharacterNumber)i;
                            break;
                        }
                    }
                }
                break;
            }
        }

        string characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]);

        characterStats.SetCurrentCharacterID(characterName);
        //currentControlCharacter[characterNames[1]].SetActive(true);
        characterDic[characterName].SetActive(true);
        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
        characterStats.InitData();
        controller.SetCharacterStats(characterStats);

        //獲取當前控制角色名稱
        foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
        {
            if (currentControlCharacterNamesSB != null)
            {
                currentControlCharacterNamesSB.Clear();
            }
            currentControlCharacterNamesSB.Append(name.Key);
            // currentControlCharacterNames = name.Key;
        }
        //角色切換時啟動訂閱的事件
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());
        //切換到戰鬥模式時啟動訂閱事件
        onNormalToBattleMode?.Invoke(currentControlCharacterNamesSB.ToString());
        //角色按鈕狀態
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);
        //characterSwitchButtons.characterSwitchSlotCanUseForStateAction.Invoke(true);

        levelSystem.SetLevelSystemData();
        SetCharacterColliderSize();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機

    }
    private void SwitchBattleCharacterInput()
    {
        if (GameManager.Instance.GetCurrentState() == (int)GameState.Battle)
        {
            if (input.CharacterSwitch1)
            {
                SwitchCharacterInBattle(1);
            }
            else if (input.CharacterSwitch2)
            {
                SwitchCharacterInBattle(2);
            }
            else if (input.CharacterSwitch3)
            {
                SwitchCharacterInBattle(3);
            }
        }
    }

    public PlayerSkillManager GetSkillManager()
    {
        if (currentSkillManager != null)
        {
            return currentSkillManager;
        }
        else
        {
            return null;
        }
    }
    public void StartNormalModeHPZeroCor()
    {
        StartCoroutine(NormalModeHPZero());
    }
    private IEnumerator NormalModeHPZero()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Normal) //一般模式只能控制主角
        {
            yield return Yielders.GetWaitForSeconds(1f);
            SwitchGameOverEvent?.Invoke();
            GameManager.Instance.SetState(GameState.GameOver); //直接遊戲結束
        }
    }
    bool hasAnotherCharacter;
    /// <summary>
    /// 這裡控制遊戲結束GameManager狀態轉換
    /// </summary>
    public void DownStateEnd()
    {
        hasAnotherCharacter = false;
        if(GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            //檢查隊伍還有沒有角色剩餘HP足夠可以更換 有則更換
            //遍歷目前隊伍列表(找到隊伍角色名)
            for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
            {
                if (!partyData.currentParty[i].IsNullOrWhitespace())
                {
                    //檢查對應角色HP是否足夠(從0開始找,用名字來查找角色狀態SO)
                    for (int j = 1; j < characterStats.characterData.Count; j++)
                    {
                        if (characterStats.characterData[j].characterName == partyData.currentParty[i])
                        {
                            hasAnotherCharacter = true;
                            if (characterStats.characterData[j].currentHealth > 0)
                            {
                                //有則更換
                                SwitchCharacterInBattle(i);
                                DownSwitchEnd?.Invoke();
                                return;
                            }
                        }
                    }
                }
            }
            if (!hasAnotherCharacter) //沒有角色可更換 遊戲結束
            {
                GameManager.Instance.SetState(GameState.GameOver); //遊戲結束
            }
        }
    }
    private void SetCharacterColliderSize()
    {
        this.playerCollider2D.size = characterColliderSize[currentControlCharacterNamesSB.ToString()];
        this.playerCollider2D.offset = characterColliderOffset[currentControlCharacterNamesSB.ToString()];
    }
    private void OnGameOver(GameState newGameState)
    {
        if (newGameState == GameState.GameOver)
        {
            gameOverPanel.gameObject.SetActive(true);
        }
    }
}
