using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CharacterData/SkillData", fileName = "SkillData")]
public class SkillDataSO : ScriptableObject
{
    public int skillLevel;
    public int skillCoolDown;
}
