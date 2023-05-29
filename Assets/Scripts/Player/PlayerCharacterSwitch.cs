using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 玩家控制角色切換
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour
{
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //隊伍資料比對用
    //public List<string> characterNames = new List<string>();
    //public List<GameObject> characterObjs = new List<GameObject>();
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();

    public static BattleCurrentCharacterNumber battleCurrentCharacterNumber;

    private PlayerInput input;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterStats characterStats;
    private PlayerPartyManager partyManager;

    /// <summary>
    /// 當前控制角色
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// AI控制友方角色
    /// </summary>
    public Dictionary<string, GameObject> subControlCharacter = new Dictionary<string, GameObject>();

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        characterStats = GetComponent<PlayerCharacterStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        partyManager = GetComponent<PlayerPartyManager>();
        StartSetCharacter("Niru");
        //if (!currentControlCharacter.ContainsKey("Niru"))
        //{
        //    SwitchMainCharacter();
        //}

    }
    private void Update()
    {
        SwitchBattleCharacterInput();
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
        if (number > partyManager.partys.Count)
        {
            Debug.Log("隊伍編號" + number + "沒有角色");
            return;
        }
        string characterName = partyManager.partys[(int)battleCurrentCharacterNumber];
        characterDic[characterName].SetActive(false);

        battleCurrentCharacterNumber = (BattleCurrentCharacterNumber)number;
        characterName = partyManager.partys[(int)battleCurrentCharacterNumber];
        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); //目前是固定的之後可以用一個常數儲存最後使用的角色
        //currentControlCharacter[characterName].SetActive(true);
        characterDic[characterName].SetActive(true);
        characterStats.currentCharacterID = 0;
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
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
    }

    /// <summary>
    /// 非戰鬥模式切換回主角
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (partyManager.partys.Count <= 0)
        {
            Debug.Log("隊伍沒有成員無法切換至戰鬥模式");//可以改為UI顯示
        }

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

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// 戰鬥模式切換到戰鬥角色
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);

        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.First;
        string characterName = partyManager.partys[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); //目前是固定的之後可以用一個常數儲存最後使用的角色
         //currentControlCharacter[characterNames[1]].SetActive(true);
        characterDic[characterName].SetActive(true);
        characterStats.currentCharacterID = 0;
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
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機

    }
    private void SwitchBattleCharacterInput()
    {
        if (GameManager.GetInstance().GetCurrentState() == (int)GameManager.GameState.Battle)
        {
            if (input.ChangeCharacter1)
            {
                SwitchCharacterInBattle(1);
            }
            else if (input.ChangeCharacter2)
            {
                SwitchCharacterInBattle(2);
            }
            else if (input.ChangeCharacter3)
            {
                SwitchCharacterInBattle(3);
            }
        }
    }
}
