using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ねよà︹ち传
/// </summary>
public class SubCharacterSwitch : SerializedMonoBehaviour
{
    public string currentSubCharacterNames;
    public System.Text.StringBuilder currentSubCharacterNamesSB = new System.Text.StringBuilder();

    public string[] subCharacterNames;
    public GameObject[] subCharacterObjs;
    public Dictionary<string, GameObject> characterDic = new Dictionary<string, GameObject>();

    private SubCharacterStateMachine stateMachine;

    /// <summary>
    /// AI北ねよà︹
    /// </summary>
    public Dictionary<string, GameObject> subControlCharacter = new Dictionary<string, GameObject>();

    private void Awake()
    {
        stateMachine = GetComponent<SubCharacterStateMachine>();
        StartSetCharacter("Niru");
        //StartSetCharacter(0);
    }
    /// <summary>
    /// ﹍て
    /// </summary>
    public void StartSetCharacter(int characterId)
    {
        subControlCharacter.Add(subCharacterNames[characterId], subCharacterObjs[characterId]);
        foreach (KeyValuePair<string, GameObject> name in subControlCharacter)
        {
            currentSubCharacterNamesSB.Append(name.Key);
        }
    }
    public void StartSetCharacter(string characterName)
    {
        subControlCharacter.Add(characterName, characterDic[characterName]);
        foreach (KeyValuePair<string, GameObject> name in subControlCharacter)
        {
            currentSubCharacterNamesSB.Append(name.Key);
        }
        currentSubCharacterNames = currentSubCharacterNamesSB.ToString();
    }

    public void StartPos(Vector3 startPos)
    {
        this.transform.position = startPos;
    }
    public void BattleModeSubCharacterEnable()
    {
        //秨币徊à︹
        if (subControlCharacter.Count > 0)
        {
            subControlCharacter.Clear();
        }

        //subControlCharacter.Add(subCharacterNames[0], subCharacterObjs[0]);
        subControlCharacter.Add(currentSubCharacterNames, characterDic[currentSubCharacterNames]);

        foreach (KeyValuePair<string, GameObject> name in subControlCharacter)
        {
            if (currentSubCharacterNamesSB != null)
            {
                currentSubCharacterNamesSB.Clear();
            }
            currentSubCharacterNamesSB.Append(name.Key);
        }
        currentSubCharacterNames = currentSubCharacterNamesSB.ToString();
        //subCharacterObjs[0].SetActive(true);
        subControlCharacter[currentSubCharacterNames].SetActive(true);

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(SubCharacterState_Idle)); //ち传à︹篈诀
    }
    public void BattleModeSubCharacterDisable()
    {
        subControlCharacter.Clear();
        //subControlCharacter.Add(subCharacterNames[0], subCharacterObjs[0]);
        currentSubCharacterNamesSB.Clear();

        subCharacterObjs[0].SetActive(false);

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(SubCharacterState_Idle)); //ち传à︹篈诀
    }
}
