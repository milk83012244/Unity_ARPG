using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ������������ƭ�
/// </summary>
[CreateAssetMenu(menuName = "Data/CharacterData/CharacterAttackData", fileName = "CharacterAttackData")]
public class CharacterAttackDataSO : SerializedScriptableObject
{
    public ElementType elementType;

    [Header("��¦�ƭ�")]
    public float attackRange;
    public float coolDown;
    public float minDamage;
    public float maxDamage;
    public float attackSpeed;
    public float stunValue; //�����ؼмW�[���w����
    public float knockbackValue; //���h��(���h���j�p)
    public float USkillAddValue;
    [Header("�ݩʥ[��")]
    public Dictionary<ElementType,float> elementDamages = new Dictionary<ElementType,float>();
    [Header("�ޯ�[��")]
    public float skill1Multplier;
    public float skill1StunValueMultplier;
    public float skill1knockbackValueMultplier;
    public float skill2Multplier;
    public float skill2StunValueMultplier;
    public float skill2knockbackValueMultplier;
    [Header("�аO�[��")]
    public float markMultplier;
    [Header("��L�[��")]
    public float subMultplier;
    [Header("�z��")]
    public float criticalMultplier;//�z�����v
    public float criticalChance;//�z���v

}
