using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��@����ޯ�޲z�� ��K��L�a������ޯ�SO�����e
public class PlayerSkillManager : MonoBehaviour
{
    private PlayerCharacterSwitch characterSwitch;

    public string currentCharacterName;

    public List<SkillDataSO> skills;

    private void Awake()
    {
        characterSwitch = GetComponentInParent<PlayerCharacterSwitch>();
    }
    private void Start()
    {
        currentCharacterName = characterSwitch.currentControlCharacterNamesSB.ToString();
    }
}
