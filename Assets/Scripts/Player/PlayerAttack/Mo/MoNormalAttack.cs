using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 角色攻擊控制管理
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

    //主攻擊是否有賦予屬性
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
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;

            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
            if (defander != null)
            {
                Enemy enemyUnit = defander.GetComponent<Enemy>();
                EnemyUnitType1 enemyUnitType1;
                EnemyUnitType2 enemyUnitType2;
                EnemyBoss1Unit enemyBoss1Unit;

                #region 可傷害敵人共通
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
                        enemyUnit.SetMark(MarkType.Mo); //設置標記
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
                //增加必殺技集氣值
                characterStats.CurrnetUSkillValue += characterStats.attackData[characterStats.currentCharacterID].USkillAddValue;

                if (liaSkill2RotateEffect.gameObject.activeSelf)//Lia的技能2附加屬性
                {
                    characterStats.TakeSubDamage(characterStats, defander, liaSkill2RotateEffect.element, FromOthersName: liaSkill2RotateEffect.characterName);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, liaSkill2RotateEffect.element, isSub: true);
                    defander.characterElementCounter.AddElementCount(liaSkill2RotateEffect.element, 1);
                }
                CinemachineShake.GetInstance().ShakeCamera(0.4f, 0.2f);//攝影機震動
                //生成擊中特效
                SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
                if (input.currentDirection == 3)
                {
                    slashHitEffect.transform.localScale = new Vector3(-1, 1, 1);
                }
                #endregion

                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;

                        //觸發敵人受擊狀態
                        enemyUnitType1.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType1.StartFlash();
                        //賦予敵人硬直值
                        if (PlayerState_Attack3.isAttack3)
                            if (enemyUnitType1.canStun)
                                characterStats.TakeStunValue(characterStats, defander, freeMul: 1.2f);
                            else
                            if (enemyUnitType1.canStun)
                                characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;

                        //觸發敵人受擊狀態
                        enemyUnitType2.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType2.StartFlash();
                        //賦予敵人硬直值
                        if (PlayerState_Attack3.isAttack3)
                            if (enemyUnitType2.canStun)
                                characterStats.TakeStunValue(characterStats, defander, freeMul: 1.2f);
                            else
                            if (enemyUnitType2.canStun)
                                characterStats.TakeStunValue(characterStats, defander);
                        break;
                    case 1001: // boss_1
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;

                        //觸發敵人受擊狀態
                        enemyBoss1Unit.DamageByPlayer();
                        ////敵人閃爍效果
                        //enemyBoss1Unit.StartFlash();
                        //賦予敵人硬直值
                        if (PlayerState_Attack3.isAttack3)
                            if (enemyBoss1Unit.canStun)
                                characterStats.TakeStunValue(characterStats, defander, freeMul: 1.2f);
                            else
                            if (enemyBoss1Unit.canStun)
                                characterStats.TakeStunValue(characterStats, defander);
                        break;
                }

                //造成敵人擊退
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;

                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;

                        if (PlayerState_Attack3.isAttack3)
                            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= 1.2f);
                        else
                            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;

                        if (PlayerState_Attack3.isAttack3)
                            enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= 1.2f);
                        else
                            enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue);
                        break;
                    case 1001: // boss_1
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                        if (PlayerState_Attack3.isAttack3)
                            enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= 1.2f);
                        else
                            enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue);
                        break;
                }

            }
        }
    }
}
