using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill2Attack : MonoBehaviour
{
    public SkillDataSO skill;
    public PlayerCharacterStats characterStats;
    public PlayerInput playerInput;

    [SerializeField] private Transform effectParent;
    [SerializeField] private GameObject skill2EffectPrefeb;
    private ObjectPool<MoSkill2Effect> skill2EffectPool;

    public int currentDirection;
    public int maxUses= 2;
    public int currentUses;

    private bool activeMarkAttack;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }

    private void Start()
    {
        skill2EffectPool = ObjectPool<MoSkill2Effect>.GetInstance();
        skill2EffectPool.InitPool(skill2EffectPrefeb, 5, effectParent);
    }

    public void StartSkillCoolDown()
    {
        if (currentUses < maxUses)
        {
            StartSpawnEffect();
            currentUses++;
            if (currentUses == 1)
            {
                StartCoroutine(SkillCoolDown(1));
            }
            else if (currentUses == 2)
            {
                StartCoroutine(SkillCoolDown(2));
            }
        }
    }
    private IEnumerator SkillCoolDown(float timeMul)
    {
        if (currentUses >= 2)
        {
            playerInput.canSkill2 = false;
        }

        yield return Yielders.GetWaitForSeconds(skill.skillCoolDown * timeMul);

        if (currentUses > 0)
        {
            currentUses -=1;
            playerInput.canSkill2 = true;
        }
    }

    public void StartSpawnEffect()
    {
        StartCoroutine(SpawnEffect());
    }
    private IEnumerator SpawnEffect()
    {
        while (PlayerState_Skill2.isMoSkill2)
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
            EnemyUnits enemyUnit = defander.GetComponent<EnemyUnits>();

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
            if (activeMarkAttack)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill: true);
            }
            else
            {
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isSkill: true);
            }
            enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
        }
    }
}
