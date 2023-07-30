using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//單一角色技能管理器 方便其他地方獲取技能SO的內容
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
