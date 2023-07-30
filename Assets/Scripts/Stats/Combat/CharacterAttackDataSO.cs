using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 角色攻擊相關數值
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : SerializedScriptableObject
{
    public ElementType elementType;

    [Header("基礎數值")]
    public float attackRange;
    public float coolDown;
    public float minDamage;
    public float maxDamage;
    public float attackSpeed;
    public float stunValue; //攻擊目標增加的硬直值
    public float knockbackValue; //擊退值(擊退的大小)
    public float USkillAddValue;
    [Header("屬性加成")]
    public Dictionary<ElementType,float> elementDamages = new Dictionary<ElementType,float>();
    [Header("技能加成")]
    public float skill1Multplier;
    public float skill1StunValueMultplier;
    public float skill1knockbackValueMultplier;
    public float skill2Multplier;
    public float skill2StunValueMultplier;
    public float skill2knockbackValueMultplier;
    [Header("標記加成")]
    public float markMultplier;
    [Header("其他加成")]
    public float subMultplier;
    [Header("爆擊")]
    public float criticalMultplier;//爆擊倍率
    public float criticalChance;//爆擊率

}
