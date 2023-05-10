using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CharacterData/SkillData", fileName = "SkillData")]
public class SkillDataSO : ScriptableObject
{
    public int skillLevel;
    public float skillDuration;
    public float skillCoolDown;
}
