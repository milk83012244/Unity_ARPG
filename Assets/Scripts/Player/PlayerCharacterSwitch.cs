using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ���a��������
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour
{
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //�����Ƥ���
    //public List<string> characterNames = new List<string>();
    //public List<GameObject> characterObjs = new List<GameObject>();
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();

    public static BattleCurrentCharacterNumber battleCurrentCharacterNumber;

    private PlayerInput input;
    private PlayerStateMachine stateMachine;
    private PlayerCharacterStats characterStats;
    private PlayerPartyManager partyManager;

    /// <summary>
    /// ��e�����
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// AI����ͤ訤��
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
        if (number > partyManager.partys.Count)
        {
            Debug.Log("����s��" + number + "�S������");
            return;
        }
        string characterName = partyManager.partys[(int)battleCurrentCharacterNumber];
        characterDic[characterName].SetActive(false);

        battleCurrentCharacterNumber = (BattleCurrentCharacterNumber)number;
        characterName = partyManager.partys[(int)battleCurrentCharacterNumber];
        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); //�ثe�O�T�w������i�H�Τ@�ӱ`���x�s�̫�ϥΪ�����
        //currentControlCharacter[characterName].SetActive(true);
        characterDic[characterName].SetActive(true);
        characterStats.currentCharacterID = 0;
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
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
    }

    /// <summary>
    /// �D�԰��Ҧ������^�D��
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (partyManager.partys.Count <= 0)
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

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// �԰��Ҧ�������԰�����
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);

        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.First;
        string characterName = partyManager.partys[(int)battleCurrentCharacterNumber];

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterName, characterDic[characterName]); //�ثe�O�T�w������i�H�Τ@�ӱ`���x�s�̫�ϥΪ�����
         //currentControlCharacter[characterNames[1]].SetActive(true);
        characterDic[characterName].SetActive(true);
        characterStats.currentCharacterID = 0;
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
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��

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
