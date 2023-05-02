using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a��������
/// </summary>
public class PlayerCharacterSwitch : MonoBehaviour
{
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    public string[] characterNames;
    public GameObject[] characterObjs;

    private PlayerStateMachine stateMachine;
    private CharacterStatsDataMutiMono characterStatsData;

    /// <summary>
    /// ��e�����
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// AI����ͤ訤��
    /// </summary>
    public Dictionary<string, GameObject> subControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// ����
    /// </summary>
    private Dictionary<int, string> partys = new Dictionary<int, string>();

    private void Awake()
    {
        characterStatsData = GetComponent<CharacterStatsDataMutiMono>();
        stateMachine = GetComponent<PlayerStateMachine>();
        StartSetCharacter(0);
        //if (!currentControlCharacter.ContainsKey("Niru"))
        //{
        //    SwitchMainCharacter();
        //}

        partys.Clear();
    }
    public void StartSetCharacter(int characterId)
    {
        currentControlCharacter.Add(characterNames[characterId], characterObjs[characterId]);
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
    public void SwitchCharacterInBattle()
    {

    }

    /// <summary>
    /// �D�԰��Ҧ������^�D��
    /// </summary>
    public void SwitchMainCharacterInNormal()
    {
        if (currentControlCharacter.Count > 0)
        {
            currentControlCharacter.Clear();
        }

        for (int i = 0; i < characterObjs.Length; i++)
        {
            if (i == 0)
            {
                if (currentControlCharacter.Count == 0)
                {
                    currentControlCharacter.Add(characterNames[i], characterObjs[i]);
                    currentControlCharacter[characterNames[0]].SetActive(true);
                    //�����e�����W��
                    foreach (KeyValuePair<string, GameObject> name in currentControlCharacter)
                    {
                        if (currentControlCharacterNamesSB != null)
                        {
                            currentControlCharacterNamesSB.Clear();
                        }
                        currentControlCharacterNamesSB.Append(name.Key);
                        //currentControlCharacterNames = name.Key;
                    }
                }
            }
            else
            {
                characterObjs[i].SetActive(false); //������L����
            }
        }
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //�������⪬�A��
    }

    /// <summary>
    /// �԰��Ҧ�������԰�����
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterObjs[0].SetActive(false);

        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterNames[1], characterObjs[1]);
        currentControlCharacter[characterNames[1]].SetActive(true);
        characterStatsData.currentCharacterID = 1;
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
}
