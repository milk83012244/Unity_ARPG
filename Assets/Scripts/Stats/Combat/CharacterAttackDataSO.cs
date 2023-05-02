using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ƭ�
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : ScriptableObject
{
    public float attackRange;
    public float skillRange;
    public float coolDown;
    public int minDamage;
    public int maxDamage;

    //�ݩʶˮ`
    public float fireElementDamage;
    public float iceElementDamage;
    public float windElementDamage;
    public float electricElementDamage;
    public float darkElementDamage;
    public float lightElementDamage;

    public float criticalMultplier;//�z�����v
    public float criticalChance;//�z���v
}
