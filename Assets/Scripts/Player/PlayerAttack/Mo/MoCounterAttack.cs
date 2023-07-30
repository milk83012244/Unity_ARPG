using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoCounterAttack : MonoBehaviour
{
    private PlayerCharacterStats characterStats;
    private PlayerEffectSpawner playerEffectSpawner;
    [HideInInspector]public PlayerInput playerInput;

    private bool activeMarkAttack;

    //是否有賦予屬性
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
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
            if (activeMarkAttack)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isCounter: true);
            }
            else
            {
                characterStats.TakeDamage(characterStats, defander, characterStats.isCritical, isCounter: true);
            }
            enemyUnit.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);

            CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.1f);//攝影機震動

            SlashHitEffect slashHitEffect = playerEffectSpawner.SlashHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);
            if (playerInput.currentDirection == 3)
            {
                slashHitEffect.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }
}
