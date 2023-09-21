using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// �����������޲z
/// </summary>
public class MoNormalAttack : MonoBehaviour
{
    private PlayerCharacterStats characterStats;
    private PlayerInput input;
    private PlayerEffectSpawner playerEffectSpawner;

    public LiaSkill2RotateEffect liaSkill2RotateEffect;

    public bool isMarkAttack;
    bool activeMarkAttack;

    public int attackCount = 0;

    //�D�����O�_���ᤩ�ݩ�
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;
    private void OnEnable()
    {
        isMarkAttack = false;
    }
    private void Awake()
    {
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        input = GetComponentInParent<PlayerInput>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
            if (defander != null)
            {
                Enemy enemyUnit = defander.GetComponent<Enemy>();
                EnemyUnitType1 enemyUnitType1 = new EnemyUnitType1();
                EnemyUnitType2 enemyUnitType2 = new EnemyUnitType2();
                EnemyBoss1Unit enemyBoss1Unit = new EnemyBoss1Unit();

                if (PlayerState_Attack3.isAttack3)
                {
                    isMarkAttack = true;
                }
                if (isMarkAttack)
                {
                    if (enemyUnit.isMarked)
                    {
                        enemyUnit.ClearMark();
                        activeMarkAttack = true;
                    }
                    else
                    {
                        enemyUnit.SetMark(MarkType.Mo); //�]�m�аO
                        activeMarkAttack = false;
                    }
                }
                if (fireElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Fire);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Fire);
                }
                else if (iceElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Ice);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Ice);
                }
                else if (windElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Wind);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                }
                else if (thunderElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Thunder);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Thunder);
                }
                else if (lightElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Light);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Light);
                }
                else if (darkElement)
                {
                    characterStats.TakeDamage(characterStats, defander, ElementType.Dark);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Dark);
                }
                if (isMarkAttack && activeMarkAttack)
                {
                    characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                    enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                    characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                }
                else
                {
                    if (PlayerState_Attack3.isAttack3)
                    {
                        characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, freeMul: 1.2f);
                    }
                    else
                    {
                        characterStats.TakeDamage(characterStats, defander, characterStats.isCritical);
                    }
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                }

                //�W�[�����޶����
                characterStats.CurrnetUSkillValue += characterStats.attackData[characterStats.currentCharacterID].USkillAddValue;

                if (liaSkill2RotateEffect.gameObject.activeSelf)//Lia���ޯ�2���[�ݩ�
                {
                    characterStats.TakeSubDamage(characterStats, defander, liaSkill2RotateEffect.element, FromOthersName: liaSkill2RotateEffect.characterName);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, liaSkill2RotateEffect.element, isSub: true);
                    defander.characterElementCounter.AddElementCount(liaSkill2RotateEffect.element, 1);
                }
                CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.1f);//��v���_��
                                                                       //�ͦ������S��
                SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
                if (input.currentDirection == 3)
                {
                    slashHitEffect.transform.localScale = new Vector3(-1, 1, 1);
                }

                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;

                        //Ĳ�o�ĤH�������A
                        enemyUnitType1.DamageByPlayer();
                        //�ĤH�{�{�ĪG
                        enemyUnitType1.StartFlash();
                        //�ᤩ�ĤH�w����
                        if (PlayerState_Attack3.isAttack3)
                            if (enemyUnitType1.canStun)
                                characterStats.TakeStunValue(characterStats, defander, freeMul: 1.2f);
                            else
                            if (enemyUnitType1.canStun)
                                characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;

                        //Ĳ�o�ĤH�������A
                        enemyUnitType2.DamageByPlayer();
                        //�ĤH�{�{�ĪG
                        enemyUnitType2.StartFlash();
                        //�ᤩ�ĤH�w����
                        if (PlayerState_Attack3.isAttack3)
                            if (enemyUnitType2.canStun)
                                characterStats.TakeStunValue(characterStats, defander, freeMul: 1.2f);
                            else
                            if (enemyUnitType2.canStun)
                                characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 1001: // boss_1

                        break;
                }

                //�y���ĤH���h
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;

                switch (enemyUnit.typeID)
                {
                    case 1:
                        if (PlayerState_Attack3.isAttack3)
                            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= 1.2f);
                        else
                            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue);
                        break;
                    case 2:
                        if (PlayerState_Attack3.isAttack3)
                            enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= 1.2f);
                        else
                            enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue);
                        break;
                }

            }
        }
    }
}
