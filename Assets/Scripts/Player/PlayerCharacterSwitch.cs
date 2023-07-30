using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 玩家控制角色切換
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour
{
    public PartyDataSO partyData;
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //隊伍資料比對用
    //public List<string> characterNames = new List<string>();
    //public List<GameObject> characterObjs = new List<GameObject>();
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();

    public static BattleCurrentCharacterNumber battleCurrentCharacterNumber;

    public int currentUseCharacterID;

    public AttackButtons attackButtons;
    public CharacterSwitchButtons characterSwitchButtons;

    private PlayerInput input;
    private PlayerController controller;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterStats characterStats;
    private PlayerCooldownController cooldownController;
    private PlayerLevelSystem levelSystem;

    [HideInInspector] public PlayerSkillManager currentSkillManager;

    public delegate void PlayerSwitchHander(string characterName);

    public event PlayerSwitchHander onNormalToBattleMode;
    public event PlayerSwitchHander onBattleToNormalMode;
    public event PlayerSwitchHander onCharacterSwitch;

    /// <summary>
    /// 當前控制角色
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<PlayerController>();
        characterStats = GetComponent<PlayerCharacterStats>();
        stateMachine = GetComponent<PlayerStateMachine>();
        cooldownController = GetComponent<PlayerCooldownController>();
        levelSystem = GetComponent<PlayerLevelSystem>();

        StartSetCharacter("Niru");
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
        if (!characterSwitchButtons.canSwitch)
        {
            Debug.Log("當前無法切換角色");
            return;
        }

        if (number > partyData.currentParty.Count)
        {
            Debug.Log("隊伍編號" + number + "沒有角色");
            return;
        }
        if (!characterSwitchButtons.characterSwitchSlots[number].gameObject.activeSelf)
        {
            Debug.Log("欄位" + number + "沒有角色");
            return;
        }
        string characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        characterDic[characterName].SetActive(false);

        battleCurrentCharacterNumber = (BattleCurrentCharacterNumber)number;
        characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); 
        //currentControlCharacter[characterName].SetActive(true);
        characterDic[characterName].SetActive(true);

        cooldownController.CharacterSwitchCooldownTrigger.Invoke(characterName); //角色切換CD計時
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);

        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
        characterStats.SetCurrentCharacterID(characterName);
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

        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());

        levelSystem.SetLevelSystemData();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
    }

    /// <summary>
    /// 非戰鬥模式切換回主角
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (partyData.currentParty.Count <= 0)
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
        characterStats.currentCharacterID = 0;

        onBattleToNormalMode?.Invoke(currentControlCharacterNamesSB.ToString());
        characterSwitchButtons.SetCurrnetUseCharacter("");

        levelSystem.SetLevelSystemData();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// 戰鬥模式切換到戰鬥角色
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);//之後可以改成一般模式當前使用角色

        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.First;
        string characterName = partyData.currentParty[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); 
         //currentControlCharacter[characterNames[1]].SetActive(true);
        characterDic[characterName].SetActive(true);
        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
        characterStats.SetCurrentCharacterID(characterName);
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

        onNormalToBattleMode?.Invoke(currentControlCharacterNamesSB.ToString());
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);
        characterSwitchButtons.characterSwitchSlotCanUseForStateAction.Invoke(true);

        levelSystem.SetLevelSystemData();

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
}
