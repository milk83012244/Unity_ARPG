using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill2Attack : MonoBehaviour
{
    private PlayerSkillManager skillManager;

    [HideInInspector] public SkillDataSO skillData;
    public PlayerCharacterStats characterStats;
    public PlayerInput playerInput;
    public PlayerCooldownController cooldownController;
    public AttackButtons attackButtons;
    public LiaSkill2RotateEffect liaSkill2RotateEffect;

    private PlayerEffectSpawner playerEffectSpawner;

    [SerializeField] private Transform effectParent;
    [SerializeField] private GameObject skill2EffectPrefeb;
    private ObjectPool<MoSkill2Effect> skill2EffectPool;

    public int currentDirection;
    public int maxUses;
    public int currentUses;

    private bool activeMarkAttack;

    private void OnEnable()
    {
        PlayerState_Skill2.isSkill2Enter = false;
    }
    private void OnDisable()
    {
        PlayerState_Skill2.isSkill2Enter = true;
    }
    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }

    private void Start()
    {
        skillData = skillManager.skills[1];
        maxUses = skillData.skillUseCount;
        skill2EffectPool = ObjectPool<MoSkill2Effect>.Instance;
        skill2EffectPool.InitPool(skill2EffectPrefeb, 5, effectParent);
    }
    public float tempTimeCount;

    /// <summary>
    /// 使用每次技能之間的間隔計算
    /// </summary>
    private IEnumerator TimeCount()
    {
        tempTimeCount = 0;
        while (currentUses !=2 && tempTimeCount<=1)
        {
            if (tempTimeCount <= 1)
            {
                tempTimeCount += Time.deltaTime;
            }
            yield return null;
        }
    }

    public void StartSkillCoolDown()
    {
        if (currentUses < maxUses)
        {
            StartSpawnEffect();
            currentUses++;
            attackButtons.Skill2CountIconRemove();
            if (currentUses == 1)
            {
                StartCoroutine(TimeCount());
                StartCoroutine(SkillCoolDown(1));
            }
            else if (currentUses == 2)
            {
                StartCoroutine(SkillCoolDown(2 -tempTimeCount));
            }
        }
    }
    private IEnumerator SkillCoolDown(float timeMul)
    {
        if (currentUses >= 2)
        {
            playerInput.canSkill2[characterStats.currentCharacterID] = false;
        }

        yield return Yielders.GetWaitForSeconds(skillData.skillCoolDown * timeMul);
        if (currentUses > 0)
        {
            currentUses -=1;
            attackButtons.Skill2CountIconAdd(characterStats.characterData[characterStats.currentCharacterID].characterName);
            playerInput.canSkill2[characterStats.currentCharacterID] = true;
        }
    }

    public void StartSpawnEffect()
    {
        StartCoroutine(SpawnEffect());
    }
    private IEnumerator SpawnEffect()
    {
        while (PlayerState_Skill2.isSkill2Enter)
        {
            MoSkill2Effect skill2EffectPrafabObj = skill2EffectPool.Spawn(transform.position, effectParent);
            skill2EffectPrafabObj.transform.localPosition = this.transform.position;
            skill2EffectPrafabObj.GetComponent<MoSkill2Effect>().moSkill2Attack = this.GetComponent<MoSkill2Attack>();
            yield return Yielders.GetWaitForSeconds(0.1f);
        }
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
                EnemyUnitType1 enemyUnitType1;
                EnemyUnitType2 enemyUnitType2;
                EnemyBoss1Unit enemyBoss1Unit;

                #region 可傷害敵人共通
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
                if (liaSkill2RotateEffect.gameObject.activeSelf)//Lia的技能2附加屬性
                {
                    characterStats.TakeSubDamage(characterStats, defander, liaSkill2RotateEffect.element, FromOthersName: liaSkill2RotateEffect.characterName);
                    enemyUnit.SpawnDamageText(characterStats.currentDamage, liaSkill2RotateEffect.element, isSub: true);
                    defander.characterElementCounter.AddElementCount(liaSkill2RotateEffect.element, 2);
                }

                if (activeMarkAttack)
                {
                    characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                    enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                    characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill2: true);
                }
                else
                {
                    characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill2: true);
                }
                enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
                CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.18f);//攝影機震動
                //生成擊中特效
                SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
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
                        if (enemyUnitType1.canStun)
                            characterStats.TakeStunValue(characterStats, defander, isSkill2: true);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;
                        //觸發敵人受擊狀態
                        enemyUnitType2.DamageByPlayer();
                        //敵人閃爍效果
                        enemyUnitType2.StartFlash();
                        //賦予敵人硬直值
                        if (enemyUnitType2.canStun)
                            characterStats.TakeStunValue(characterStats, defander, isSkill2: true);
                        break;
                    case 1001:
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                        //觸發敵人受擊狀態
                        enemyBoss1Unit.DamageByPlayer();
                        //敵人閃爍效果
                        //enemyBoss1Unit.StartFlash();
                        //賦予敵人硬直值
                        if (enemyBoss1Unit.canStun)
                            characterStats.TakeStunValue(characterStats, defander, isSkill2: true);
                        break;
                }
                //造成敵人擊退
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                float knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;

                switch (enemyUnit.typeID)
                {
                    case 1:
                        enemyUnitType1 = enemyUnit as EnemyUnitType1;
                        enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue *= characterStats.attackData[characterStats.currentCharacterID].skill2knockbackValueMultplier);
                        break;
                    case 2:
                        enemyUnitType2 = enemyUnit as EnemyUnitType2;
                        enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue *= characterStats.attackData[characterStats.currentCharacterID].skill2knockbackValueMultplier);
                        break;
                    case 1001:
                        enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                        enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue *= characterStats.attackData[characterStats.currentCharacterID].skill2knockbackValueMultplier);
                        break;
                }
            }
        }
    }
}
