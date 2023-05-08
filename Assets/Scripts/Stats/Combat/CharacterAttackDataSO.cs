using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻擊相關數值
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : ScriptableObject
{
    public ElementType elementType;

    public float attackRange;
    public float coolDown;
    public float minDamage;
    public float maxDamage;

    public float attackSpeed;

    //屬性傷害
    public float fireElementDamage;
    public float iceElementDamage;
    public float windElementDamage;
    public float thunderElementDamage;
    public float darkElementDamage;
    public float lightElementDamage;

    public float skill1Multplier;
    public float skill2Multplier;

    public float markMultplier;

    public float criticalMultplier;//爆擊倍率
    public float criticalChance;//爆擊率

}
