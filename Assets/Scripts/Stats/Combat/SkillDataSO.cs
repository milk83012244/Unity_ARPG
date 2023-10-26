using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/CharacterData/SkillData", fileName = "SkillData")]
public class SkillDataSO : ScriptableObject
{
    public bool hasFiring;//是否有瞄準功能
    public float skillFiringRange;//準心的移動範圍

    public int skillLevel;
    public int skillUseCount;
    public float skillDuration; //長效型技能持續時間
    public float skillCoolDown;

}
