using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoSkill1Attack : MonoBehaviour
{
    public SkillDataSO skill;
    public PlayerCharacterStats characterStats;
    public PlayerInput playerInput;

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
    }
    private void Start()
    {
        skill1EffectPool = ObjectPool<MoSkill1Effect>.GetInstance();
        skill1EffectPool.InitPool(skill1EffectPrefeb, 3, effectParent);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = UnityEngine.Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            OtherCharacterStats defander = collision.GetComponent<OtherCharacterStats>();
            TestUnit enemyUnit = defander.GetComponent<TestUnit>();
            if (playerInput.canSkill1)
            {
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
    public void StartSkillCoolDown()
    {
        StartCoroutine(SkillCoolDown());
    }
    private IEnumerator SkillCoolDown()
    {
        playerInput.canSkill1 = false;
        yield return Yielders.GetWaitForSeconds(skill.skillCoolDown);
        playerInput.canSkill1 = true;
    }
    public void SpawnSkillEffect(int currentDirection)
    {
        switch (currentDirection)
        {
            case 4: //¤W
                SpawnDir = new Vector3(0.15f, 0.8f);
                break;
            case 2: //¤U
                SpawnDir = new Vector3(-0.1f, -0.15f);
                break;
            case 1: //¥ª
                SpawnDir = new Vector3(-0.5f, 0.3f);
                break;
            case 3: //¥k
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
