using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ���a��������
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour
{
    public PartyDataSO partyData;
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //�����Ƥ���
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
    /// ��e�����
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
    ///  ��l�ƨ���
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
    /// �D�԰��Ҧ�����������
    /// </summary>
    public void SwitchCharacterInNormal()
    {

    }
    /// <summary>
    /// �԰��Ҧ�����������
    /// </summary>
    public void SwitchCharacterInBattle(int number)
    {
        if (!characterSwitchButtons.canSwitch)
        {
            Debug.Log("��e�L�k��������");
            return;
        }

        if (number > partyData.currentParty.Count)
        {
            Debug.Log("����s��" + number + "�S������");
            return;
        }
        if (!characterSwitchButtons.characterSwitchSlots[number].gameObject.activeSelf)
        {
            Debug.Log("���" + number + "�S������");
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

        cooldownController.CharacterSwitchCooldownTrigger.Invoke(characterName); //�������CD�p��
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);

        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
        characterStats.SetCurrentCharacterID(characterName);
        characterStats.ResetData();
        controller.SetCharacterStats(characterStats);

        //�����e�����W��
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
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
    }

    /// <summary>
    /// �D�԰��Ҧ������^�D��
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (partyData.currentParty.Count <= 0)
        {
            Debug.Log("����S�������L�k�����ܾ԰��Ҧ�");//�i�H�אּUI���
        }

        if (currentControlCharacter.Count > 0)
        {
            currentControlCharacter.Clear();
        }

        if (currentControlCharacter.Count == 0)
        {
            foreach (var characterName in characterDic)
            {
                if (characterName.Key == "Niru") //������L���⪫��
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
            //�����e�����W��
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
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// �԰��Ҧ�������԰�����
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);//����i�H�令�@��Ҧ���e�ϥΨ���

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

        //�����e�����W��
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
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��

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
