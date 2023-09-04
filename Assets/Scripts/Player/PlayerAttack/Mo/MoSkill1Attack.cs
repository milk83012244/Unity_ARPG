using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mo�ޯ�1���� �޲z�ˮ`�p�� CD�p�ɶ}�l
/// </summary>
public class MoSkill1Attack : MonoBehaviour
{
    private PlayerSkillManager skillManager;

    [HideInInspector]public SkillDataSO skillData;
    public PlayerCharacterStats characterStats;
    [HideInInspector] public PlayerInput playerInput;
    public LiaSkill2RotateEffect liaSkill2RotateEffect;

    private PlayerEffectSpawner playerEffectSpawner;

    //public int currentDirection;

    [SerializeField] private Transform effectParent;
    [SerializeField] private GameObject skill1EffectPrefeb;
    private ObjectPool<MoSkill1Effect> skill1EffectPool;

    private bool activeMarkAttack;
    private Vector3 SpawnDir;
    private Coroutine skillCoolDownCor;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }
    private void Start()
    {
        if (skillManager != null)
        {
            skillData = skillManager.skills[0];
        }

        skill1EffectPool = ObjectPool<MoSkill1Effect>.Instance;
        skill1EffectPool.InitPool(skill1EffectPrefeb, 3, effectParent);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
            Enemy enemyUnit = defander.GetComponent<Enemy>();

            if (enemyUnit.isMarked)
            {
                enemyUnit.ClearMark();
                activeMarkAttack = true;
            }
            else
            {
                enemyUnit.SetMark(MarkType.Mo);
                activeMarkAttack = false;
            }
            if (liaSkill2RotateEffect.gameObject.activeSelf)//Lia���ޯ�2���[�ݩ�
            {
                characterStats.TakeSubDamage(characterStats, defander, liaSkill2RotateEffect.element, FromOthersName: liaSkill2RotateEffect.characterName);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, liaSkill2RotateEffect.element, isSub: true);
                defander.characterElementCounter.AddElementCount(liaSkill2RotateEffect.element, 2, characterStats);
            }
            if (activeMarkAttack)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill1:true);
            }
            else
            {
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill1: true);
            }
            enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);

            SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
            if (playerInput.currentDirection == 3)
            {
                slashHitEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
            EnemyUnitType1 enemyUnitType1 = enemyUnit as EnemyUnitType1;
            //Ĳ�o�ĤH�������A
            enemyUnitType1.DamageByPlayer();
            //�ĤH�{�{�ĪG
            enemyUnitType1.StartFlash();
            //�y���ĤH�w��
            characterStats.TakeStunValue(characterStats, defander, isSkill1: true);
            //�y���ĤH���h
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
            enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= characterStats.attackData[characterStats.currentCharacterID].skill1StunValueMultplier);
        }
    }

    public void SpawnSkillEffect(int currentDirection)
    {
        switch (currentDirection)
        {
            case 4: //�W
                SpawnDir = new Vector3(0.15f, 0.8f);
                break;
            case 2: //�U
                SpawnDir = new Vector3(-0.1f, -0.15f);
                break;
            case 1: //��
                SpawnDir = new Vector3(-0.5f, 0.3f);
                break;
            case 3: //�k
                SpawnDir = new Vector3(0.5f, 0.3f);
                break;
        }
        MoSkill1Effect skill1EffectPrafabObj = skill1EffectPool.Spawn(transform.position, effectParent);
        skill1EffectPrafabObj.transform.localPosition = this.transform.position;
        skill1EffectPrafabObj.transform.localPosition += SpawnDir;
        if (currentDirection == 4)
        {
            skill1EffectPrafabObj.transform.rotation =  Quaternion.Euler(0, 0, -90);
        }
        else if (currentDirection == 2)
        {
            skill1EffectPrafabObj.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            skill1EffectPrafabObj.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        skill1EffectPrafabObj.GetComponent<MoSkill1Effect>().moSkill1Attack = this.GetComponent<MoSkill1Attack>();
    }
}
