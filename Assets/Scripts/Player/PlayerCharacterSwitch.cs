using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine.Events;

/// <summary>
/// ���a��������
/// </summary>
public class PlayerCharacterSwitch : SerializedMonoBehaviour, IDataPersistence
{
    public PartyDataSO partyData;
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    //�����Ƥ���
    //public List<string> characterNames = new List<string>();
    //public List<GameObject> characterObjs = new List<GameObject>();
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();
    //�U����I�����j�p
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
    /// ��e�����
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
    ///  ��l�ƨ���
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
            tipTextObject.startShowTextCor("��e�L�k��������");
            Debug.Log("��e�L�k��������");
            return;
        }

        if (number > partyData.currentParty.Count)
        {
            tipTextObject.startShowTextCor("����s��" + number + "�S������");
            Debug.Log("����s��" + number + "�S������");
            return;
        }
        if (!characterSwitchButtons.characterSwitchSlots[number].gameObject.activeSelf)
        {
            Debug.Log("���" + number + "�S������");
            return;
        }
        //�ˬd�n�󴫪�����HP�O�_��0
        for (int j = 1; j < characterStats.characterData.Count; j++)
        {
            if (characterStats.characterData[j].characterName == partyData.currentParty[number])
            {
                if (characterStats.characterData[j].currentHealth <= 0)
                {
                    tipTextObject.startShowTextCor("���" + number + "����HP��0�L�k�԰�");
                    Debug.Log("���" + number + "����HP��0�L�k�԰�");
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

        cooldownController.CharacterSwitchCooldownTrigger.Invoke(characterName); //�������CD�p��

        //������s���A
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);

        currentSkillManager = characterDic[characterName].GetComponent<PlayerSkillManager>();
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
        //��������ɱҰʭq�\���ƥ�
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());

        levelSystem.SetLevelSystemData();

        SetCharacterColliderSize();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
    }

    /// <summary>
    /// �q�԰��Ҧ������^�D��
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
        controller.SetCharacterStats(characterStats);
        //��������ɱҰʭq�\���ƥ�
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());
        //�^�@��Ҧ��Ұʭq�\���ƥ�
        onBattleToNormalMode?.Invoke(currentControlCharacterNamesSB.ToString());
        characterSwitchButtons.SetCurrnetUseCharacter("");

        levelSystem.SetLevelSystemData();

        SetCharacterColliderSize();

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
        battleCurrentCharacterNumber = BattleCurrentCharacterNumber.None;
    }

    /// <summary>
    /// �@��Ҧ�������԰��Ҧ������԰�����
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterDic["Niru"].SetActive(false);//����i�H�令�@��Ҧ���e�ϥΨ���

        //battleCurrentCharacterNumber = BattleCurrentCharacterNumber.First; //�q�Ĥ@�Ө���}�l����

        //�M���ثe����C��(��춤���W)
        for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
        {
            if (!partyData.currentParty[i].IsNullOrWhitespace())
            {
                //�ˬd�n�󴫪�����HP�O�_��0
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
        //��������ɱҰʭq�\���ƥ�
        onCharacterSwitch?.Invoke(currentControlCharacterNamesSB.ToString());
        //������԰��Ҧ��ɱҰʭq�\�ƥ�
        onNormalToBattleMode?.Invoke(currentControlCharacterNamesSB.ToString());
        //������s���A
        characterSwitchButtons.SetCurrnetUseCharacter(characterName);
        //characterSwitchButtons.characterSwitchSlotCanUseForStateAction.Invoke(true);

        levelSystem.SetLevelSystemData();
        SetCharacterColliderSize();

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
    public void StartNormalModeHPZeroCor()
    {
        StartCoroutine(NormalModeHPZero());
    }
    private IEnumerator NormalModeHPZero()
    {
        if (GameManager.Instance.CurrentGameState == GameState.Normal) //�@��Ҧ��u�౱��D��
        {
            yield return Yielders.GetWaitForSeconds(1f);
            SwitchGameOverEvent?.Invoke();
            GameManager.Instance.SetState(GameState.GameOver); //�����C������
        }
    }
    bool hasAnotherCharacter;
    /// <summary>
    /// �o�̱���C������GameManager���A�ഫ
    /// </summary>
    public void DownStateEnd()
    {
        hasAnotherCharacter = false;
        if(GameManager.Instance.CurrentGameState == GameState.Battle)
        {
            //�ˬd�����٦��S������ѾlHP�����i�H�� ���h��
            //�M���ثe����C��(��춤���W)
            for (int i = 1; i < partyData.currentParty.Keys.Count + 1; i++)
            {
                if (!partyData.currentParty[i].IsNullOrWhitespace())
                {
                    //�ˬd��������HP�O�_����(�q0�}�l��,�ΦW�r�Ӭd�䨤�⪬�ASO)
                    for (int j = 1; j < characterStats.characterData.Count; j++)
                    {
                        if (characterStats.characterData[j].characterName == partyData.currentParty[i])
                        {
                            hasAnotherCharacter = true;
                            if (characterStats.characterData[j].currentHealth > 0)
                            {
                                //���h��
                                SwitchCharacterInBattle(i);
                                DownSwitchEnd?.Invoke();
                                return;
                            }
                        }
                    }
                }
            }
            if (!hasAnotherCharacter) //�S������i�� �C������
            {
                GameManager.Instance.SetState(GameState.GameOver); //�C������
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
