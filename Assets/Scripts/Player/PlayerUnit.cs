using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家狀態效果演出相關
/// </summary>
public class PlayerUnit : MonoBehaviour
{
    private Rigidbody2D rig2D;
    private PlayerCharacterStats characterStats;
    private PlayerController playerController;

    private float knockbackDuration = 0.1f;
    private bool isKnockbackActive;

    //硬直效果
    public bool canStun;

    private void OnEnable()
    {
        characterStats.stunValueIsMaxAction += StartStunned;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        characterStats.stunValueIsMaxAction -= StartStunned;
    }
    private void Awake()
    {
        canStun = true;
        rig2D = GetComponent<Rigidbody2D>();
        characterStats = GetComponent<PlayerCharacterStats>();
        playerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// 擊退效果
    /// </summary>
    public void StartKnockback(Vector2 direction, float knockbackValue)
    {
        StartCoroutine(ApplyKnockback(direction, knockbackValue));
    }
    private IEnumerator ApplyKnockback(Vector2 direction, float knockbackValue)
    {
        isKnockbackActive = true;
        rig2D.velocity = direction * (knockbackValue - characterStats.characterData[characterStats.currentCharacterID].knockbackResistance);

        yield return Yielders.GetWaitForSeconds(knockbackDuration);

        rig2D.velocity = Vector2.zero;
        isKnockbackActive = false;
    }
    public void StartStunned(float stunTime)
    {
        StartCoroutine(Stunned(stunTime));
    }
    /// <summary>
    /// 硬值效果
    /// </summary>
    private IEnumerator Stunned(float stunTime)
    {
        playerController.StartDamageState(true);
        yield return Yielders.GetWaitForSeconds(stunTime);
        characterStats.CurrnetStunValue = 0;
        playerController.StartDamageState(false);
        StartCoroutine(StunCoolDown());
    }
    private IEnumerator StunCoolDown()
    {
        canStun = false;
        yield return Yielders.GetWaitForSeconds(characterStats.characterData[characterStats.currentCharacterID].stunCooldownTime);
        canStun = true;
    }
}
