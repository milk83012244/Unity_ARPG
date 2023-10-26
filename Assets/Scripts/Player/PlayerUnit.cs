using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// ���a���A�ĪG����
/// </summary>
public class PlayerUnit : SerializedMonoBehaviour
{
    private Rigidbody2D rig2D;
    private PlayerCharacterStats characterStats;
    private PlayerController playerController;
    private PlayerCharacterSwitch characterSwitch;
    private PlayerSkillManager skillManager;

    private float knockbackDuration = 0.1f;
    private bool isKnockbackActive;

    public Dictionary<string, bool> powerUpUSkillActive = new Dictionary<string, bool>(); //�U������򫬥����޶}�Ҫ��A

    //�w���ĪG
    public bool canStun;

    private void OnEnable()
    {
        characterStats.stunValueIsMaxAction += StartStunned;
        characterSwitch.onAnyCharacterSwitch += USkillEnd;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        characterStats.stunValueIsMaxAction -= StartStunned;
        characterSwitch.onAnyCharacterSwitch -= USkillEnd;
    }
    private void Awake()
    {
        canStun = true;
        rig2D = GetComponent<Rigidbody2D>();
        characterStats = GetComponent<PlayerCharacterStats>();
        playerController = GetComponent<PlayerController>();
    }
    public void SetPlayerSkillManager(PlayerSkillManager skillManager)
    {
        this.skillManager = skillManager;
    }

    #region �t�����A
    /// <summary>
    /// ���h�ĪG
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
    /// �w���ĪG
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
    #endregion

    #region �j�ƪ��A
    public void StartMoUSkillStartBuff()
    {
        StartCoroutine(MoUSkillStartBuff());
    }
    private IEnumerator MoUSkillStartBuff()
    {
        if (skillManager == null || skillManager.currentCharacterName != "Mo")
        {
            yield break;
        }
        //���ܪ��A
        powerUpUSkillActive["Mo"] = true;
        //�j�Ƽƭ�
        characterStats.SetAttackRate(true, 0.5f);
        //�}�Ҭ����S��

        yield return Yielders.GetWaitForSeconds(skillManager.skills[2].skillDuration);
        //�ɶ��찱��
        USkillEnd();
    }
    /// <summary>
    /// �����޵���
    /// </summary>
    public void USkillEnd()
    {
        StopAllCoroutines();
        switch (skillManager.currentCharacterName)
        {
            case "Mo":
                powerUpUSkillActive["Mo"] = false;
                characterStats.SetAttackRate(false, 0.5f);
                //���������S��
                //����Buff���A
                break;
        }
    }
    #endregion
}
