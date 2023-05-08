using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill1Attack : MonoBehaviour
{
    public SkillDataSO skill;
    public PlayerCharacterStats characterStats;
    public PlayerInput playerInput;
    public GameObject skill1EffectPrefeb;

    [SerializeField] private Transform effectParent;

    private bool activeMarkAttack;

    private Coroutine skillCoolDownCor;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
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

            SpawnSkillEffect();
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
    private void SpawnSkillEffect()
    {
        GameObject skill1EffectPrafabObj = Instantiate(skill1EffectPrefeb, effectParent);
        skill1EffectPrafabObj.transform.localPosition = this.transform.localPosition;
        skill1EffectPrafabObj.GetComponent<MoSkill1Effect>().moSkill1Attack = this.GetComponent<MoSkill1Attack>();
    }
}
