using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色戰鬥能力資料
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterBattleData", fileName = "CharacterBattleData")]
public class CharacterBattleDataSO : ScriptableObject
{
    [Header("Stats Info")]
    public int mexHealth;
    public int currentHealth;
    public int baseDefence;
    public int currentDefence;

    //屬性減免(防禦)
    public float fireElementDefence;
    public float iceElementDefence;
    public float windElementDefence;
    public float electricElementDefence;
    public float darkElementDefence;
    public float lightElementDefence;

    [Header("Level")]
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
}
