using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoSkill2Attack : MonoBehaviour
{
    public SkillDataSO skill;
    public PlayerCharacterStats characterStats;
    public PlayerInput playerInput;

    public int currentDirection;

    private bool activeMarkAttack;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
    }
    public void StartSkillCoolDown()
    {
        StartCoroutine(SkillCoolDown());
    }
    private IEnumerator SkillCoolDown()
    {
        playerInput.canSkill2 = false;
        yield return Yielders.GetWaitForSeconds(skill.skillCoolDown);
        playerInput.canSkill2 = true;
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
}
