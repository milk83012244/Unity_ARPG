using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色攻擊相關數值
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;

    //屬性傷害
    public float fireElementDamage;
    public float iceElementDamage;
    public float windElementDamage;
    public float electricElementDamage;
    public float darkElementDamage;
    public float lightElementDamage;

    public float criticalMultplier;//爆擊倍率
    public float criticalChance;//爆擊率
}
