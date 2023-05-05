using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������������ƭ�
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : ScriptableObject
{
    public ElementType elementType;

    public float attackRange;
    public float coolDown;
    public float minDamage;
    public float maxDamage;

    //�ݩʶˮ`
    public float fireElementDamage;
    public float iceElementDamage;
    public float windElementDamage;
    public float thunderElementDamage;
    public float darkElementDamage;
    public float lightElementDamage;

    public float criticalMultplier;//�z�����v
    public float criticalChance;//�z���v

}
