using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterSwitch : MonoBehaviour
{
    public string currentControlCharacterNames;
    public System.Text.StringBuilder currentControlCharacterNamesSB = new System.Text.StringBuilder();

    public string[] characterNames;
    public GameObject[] characterObjs;

    private PlayerStateMachine stateMachine;
    private PlayerSubCharacterController playerSubCharacter;

    /// <summary>
    /// 當前控制角色
    /// </summary>
    private Dictionary<string, GameObject> currentControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// AI控制友方角色
    /// </summary>
    public Dictionary<string, GameObject> subControlCharacter = new Dictionary<string, GameObject>();
    /// <summary>
    /// 隊伍
    /// </summary>
    private Dictionary<int, string> partys = new Dictionary<int, string>();

    private void Awake()
    {
        stateMachine = GetComponent<PlayerStateMachine>();
        playerSubCharacter = GetComponent<PlayerSubCharacterController>();
        BattleSubCharacter(0);
        //if (!currentControlCharacter.ContainsKey("Niru"))
        //{
        //    SwitchMainCharacter();
        //}

        partys.Clear();
    }
    public void BattleSubCharacter(int characterId)
    {
        currentControlCharacter.Add(characterNames[characterId], characterObjs[characterId]);
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
    public void SwitchCharacterInBattle()
    {

    }

    /// <summary>
    /// 非戰鬥模式切換回主角
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
                    //獲取當前控制角色名稱
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
                characterObjs[i].SetActive(false); //關閉其他角色
            }
        }
        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(PlayerState_Idle)); //切換角色狀態機
    }

    /// <summary>
    /// 戰鬥模式切換到戰鬥角色
    /// </summary>
    public void BattleModeStartSwitchCharacter()
    {
        characterObjs[0].SetActive(false);
       // playerSubCharacter.enabled = true;
        subControlCharacter.Clear();
        subControlCharacter.Add(characterNames[0], characterObjs[0]);
        currentControlCharacter.Clear();
        currentControlCharacter.Add(characterNames[1], characterObjs[1]);
        currentControlCharacter[characterNames[1]].SetActive(true);
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
}
