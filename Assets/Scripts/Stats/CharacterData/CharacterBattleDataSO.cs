using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色戰鬥能力資料
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterBattleData", fileName = "CharacterBattleData")]
public class CharacterBattleDataSO : ScriptableObject
{
    public ElementType elementType;
    public AttackType attackType;
    public int PlayerNormalAttackCount; //角色普攻次數

    [Header("Stats Info")]
    public int mexHealth;
    public int currentHealth;
    public float baseDefence;
    public float currentDefence;

    //屬性減免(防禦)
    public float fireElementDefence;
    public float iceElementDefence;
    public float windElementDefence;
    public float thunderElementDefence;
    public float darkElementDefence;
    public float lightElementDefence;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
}
