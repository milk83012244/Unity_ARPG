using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubCharacterSwitch : MonoBehaviour
{
    public System.Text.StringBuilder currentSubCharacterNamesSB = new System.Text.StringBuilder();

    public string[] subCharacterNames;
    public GameObject[] subCharacterObjs;

    private SubCharacterStateMachine stateMachine;

    /// <summary>
    /// AI控制友方角色
    /// </summary>
    public Dictionary<string, GameObject> subControlCharacter = new Dictionary<string, GameObject>();

    private void Awake()
    {
        stateMachine = GetComponent<SubCharacterStateMachine>();
        StartSetCharacter(0);
    }
    public void StartSetCharacter(int characterId)
    {
        subControlCharacter.Add(subCharacterNames[characterId], subCharacterObjs[characterId]);
        foreach (KeyValuePair<string, GameObject> name in subControlCharacter)
        {
            currentSubCharacterNamesSB.Append(name.Key);
        }
    }
    public void StartPos(Vector3 startPos)
    {
        this.transform.position = startPos;
    }
    public void BattleModeSubCharacterEnable()
    {
        //開啟輔助角色
        if (subControlCharacter.Count > 0)
        {
            subControlCharacter.Clear();
        }

        subControlCharacter.Add(subCharacterNames[0], subCharacterObjs[0]);

        foreach (KeyValuePair<string, GameObject> name in subControlCharacter)
        {
            if (currentSubCharacterNamesSB != null)
            {
                currentSubCharacterNamesSB.Clear();
            }
            currentSubCharacterNamesSB.Append(name.Key);
        }
        subCharacterObjs[0].SetActive(true);

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(SubCharacterState_Idle)); //切換角色狀態機
    }
    public void BattleModeSubCharacterDisable()
    {
        subControlCharacter.Clear();
        //subControlCharacter.Add(subCharacterNames[0], subCharacterObjs[0]);
        currentSubCharacterNamesSB.Clear();

        subCharacterObjs[0].SetActive(false);

        stateMachine.ReIbitialize();
        stateMachine.SwitchState(typeof(SubCharacterState_Idle)); //切換角色狀態機
    }
}
