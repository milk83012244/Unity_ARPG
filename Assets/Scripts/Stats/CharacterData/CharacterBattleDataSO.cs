using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 角色戰鬥能力資料
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterBattleData", fileName = "CharacterBattleData")]
public class CharacterBattleDataSO : SerializedScriptableObject
{
    [Header("角色資訊")]
    public string characterName = "";
    public int characterID;//角色唯一ID
    public CharacterDamageType damageType; //受傷類型
    public AttackType attackType;
    public int PlayerNormalAttackCount; //角色普攻次數

    [Header("基礎能力值")]
    public int mexHealth;
    public int currentHealth;
    public float baseDefence;
    public float currentDefence;
    public float currentUSkillValue;

    [Header("擊退 硬直數值")]
    public float knockbackResistance; //擊退抵抗值
    public float maxStunValue;
    public float currentStunValue; //硬直值 滿了會觸發硬直
    public float stunResistance;//硬直抵抗值
    public float stunRecovorTime;//硬直狀態回復時間
    public float stunCooldownTime;//硬直狀態冷卻時間
    public float stunValueTime;//硬直值完全消除總時長

    [Header("屬性減免")]
    public Dictionary<ElementType, float> elementDefense = new Dictionary<ElementType, float>();

    [Header("等級數值")]
    public readonly List<int> experiencePerLevel = new List<int>();
    public int currentLevel;
    public int maxLevel;
    public int baseExp;
    public int currentExp;
}
