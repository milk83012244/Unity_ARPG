using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��������� �i�y���ˮ`(�i��������)
/// </summary>
public class PlayerCharacterStats : MonoBehaviour
{
    private PlayerUnit playerUnit;
    public AttackButtons attackButtons;

    public int currentCharacterID;

    //����n�令����������⭫�s���J���������⪺���(�Φr�嵥��ȹ��x�s)
    public List<CharacterBattleDataSO> characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO> attackData = new List<CharacterAttackDataSO>();
    public List<CharacterElementCountSO> elementCountData = new List<CharacterElementCountSO>();


    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    public bool isInvincible;

    public Action<float> stunValueChangedAction;
    public Action<float> stunValueIsMaxAction;
    public Action USkillValueIsMaxAction;

    Coroutine StunValueCountCor;

    #region �qChatacterDataSOŪ����
    public int MaxHealth
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].mexHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].currentHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentHealth = value;
        }
    }
    public float BaseDefence
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].baseDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].baseDefence = value;
        }
    }
    public float CurrentDefence
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].currentDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentDefence = value;
        }
    }
    public float MaxStunValue
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].maxStunValue;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].maxStunValue = value;
        }
    }
    public float CurrnetStunValue
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].currentStunValue;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentStunValue = value;
            StopStunCount();
            if (characterData[currentCharacterID].currentStunValue > 0 && characterData[currentCharacterID].currentStunValue < characterData[currentCharacterID].maxStunValue)
                stunValueChangedAction?.Invoke(characterData[currentCharacterID].stunValueTime);

            if (characterData[currentCharacterID].currentStunValue >= characterData[currentCharacterID].maxStunValue)
            {
                characterData[currentCharacterID].currentStunValue = characterData[currentCharacterID].maxStunValue;
                Debug.Log(string.Format("<color=red>{0}</color>", characterData[currentCharacterID].characterName + "�QĲ�o�w��"));
                //�i�J�w�����A
                stunValueIsMaxAction?.Invoke(characterData[currentCharacterID].stunRecovorTime);
            }
        }
    }
    public float CurrnetUSkillValue
    {
        get
        {
            if (characterData != null && currentCharacterID != -1)
                return characterData[currentCharacterID].currentUSkillValue;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentUSkillValue = value;
            SetUSkillValueBar();
            if (characterData[currentCharacterID].currentUSkillValue >= 100)
            {
                characterData[currentCharacterID].currentUSkillValue = 100;
                Debug.Log(string.Format("<color=red>{0}</color>", characterData[currentCharacterID].characterName + "�i�H�ϥΥ�����"));
                USkillValueIsMaxAction?.Invoke();
            }
        }
    }

    #endregion
    private void OnEnable()
    {
        stunValueChangedAction += StartStunCount;
    }
    private void OnDestroy()
    {
        stunValueChangedAction -= StartStunCount;
    }
    private void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
    }

    #region �ˮ`�p��
    /// <summary>
    /// �y���ˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false, bool isCounter = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, isCritical, isSkill1, isSkill2, isCounter, freeMul) - defender.CurrentDefence, 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        #region �ո�

        if (isSkill1)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�ϥΧޯ�1��" + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        else if (isSkill2)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " �ϥΧޯ�2�� " + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        else if (isCounter)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�ϥΤ�����" + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�B�~���v��" + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " �ϥΤ@��/��������� " + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        #endregion

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
        //CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.1f);//�y���ˮ`����v���_��
    }
    /// <summary>
    /// �аO�ˮ`
    /// </summary>
    public void TakeMarkDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateMarkDamage(attacker, isCritical, freeMul) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�B�~���v��" + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " ���аO�� " + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �ƶˮ`
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, attackData[currentCharacterID].subMultplier, isCritical, freeMul) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�B�~���v��" + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " ���Ƨ����� " + defender.enemyBattleData.characterName + "���ˮ`: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �y���ݩʶˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        //��o�p�⾹
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //�������m�O
        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType, isCritical, isSkill1, isSkill2, freeMul) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        currentDamage = (int)Mathf.Round(damagefloat);
        #region �ո�
        if (isSkill1)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�ϥΧޯ�1��" + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        else if (isSkill2)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " �ϥΧޯ�2�� " + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�B�~���v��" + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " �ϥΤ@��/��������� " + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        #endregion

        //�y���ˮ`
        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    /// <summary>
    /// �ݩʰƶˮ`
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false, bool isSkill = false, string FromOthersName = "", float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //�������m�O
        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, attackData[currentCharacterID].subMultplier, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        currentDamage = (int)Mathf.Round(damagefloat);
        if (FromOthersName != "")
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", FromOthersName + "�����[�ˮ`�� " + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "�B�~���v��" + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " �����ݩʧ����� " + defender.enemyBattleData.characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI��s
        //�g�紣�ɵ�
    }
    public void TakeStunValue(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isSkill1 = false, bool isSkill2 = false, float freeMul = 0)
    {
        float defenderStunValue = defender.CurrnetStunValue;
        float attackStunValue;
        if (isSkill1)
        {
            attackStunValue = attacker.attackData[currentCharacterID].stunValue *= attackData[currentCharacterID].skill1StunValueMultplier - defender.enemyBattleData.stunResistance;
        }
        else if (isSkill2)
        {
            attackStunValue = attacker.attackData[currentCharacterID].stunValue *= attackData[currentCharacterID].skill2StunValueMultplier - defender.enemyBattleData.stunResistance;
        }
        else if (freeMul != 0)
        {
            attackStunValue = attacker.attackData[currentCharacterID].stunValue *= freeMul - defender.enemyBattleData.stunResistance;
        }
        else
        {
            attackStunValue = attacker.attackData[currentCharacterID].stunValue - defender.enemyBattleData.stunResistance;
        }

        defenderStunValue += attackStunValue;
        defender.CurrnetStunValue = defenderStunValue;
    }

    /// <summary>
    /// �Ұʻ���w�ȭ�
    /// </summary>
    public void StartStunCount(float time)
    {
        if (StunValueCountCor == null)
        {
            StunValueCountCor = StartCoroutine(StunValueCount(time));
        }
    }
    private IEnumerator StunValueCount(float time)
    {
        float timer = 0f;
        float currentStunValueCount = characterData[currentCharacterID].currentStunValue;
        while (characterData[currentCharacterID].currentStunValue > 0)
        {
            timer += Time.deltaTime;
            float t = timer / time;
            characterData[currentCharacterID].currentStunValue = Mathf.Lerp(currentStunValueCount, 0, t);

            int tempStunValue = (int)characterData[currentCharacterID].currentStunValue;

            if (tempStunValue % 10 == 0)
                Debug.Log("�ثe�w����: " + tempStunValue);

            // Debug.Log("�ثe�w����: " + characterData[currentCharacterID].currentStunValue);
            yield return null;
        }
        if (characterData[currentCharacterID].currentStunValue == 0)
        {
            yield break;
        }
    }
    public void StopStunCount()
    {
        if (StunValueCountCor!=null)
        {
            StopCoroutine(StunValueCountCor);
            StunValueCountCor = null;
        }
    }
    #endregion
    /// <summary>
    /// ��l�ƨ���ƭ�
    /// </summary>
    public void InitData()
    {

    }
    /// <summary>
    /// ��������ɭ��]�ƭ�
    /// </summary>
    public void ResetData()
    {

    }
    public void SetUSkillValueBar()
    {
        attackButtons.USkillIconAddBar.fillAmount =  characterData[currentCharacterID].currentUSkillValue / 100;
    }
    public float GetElementDefence(ElementType elementType)
    {
        return characterData[currentCharacterID].elementDefense[elementType];
    }
    public void SetCurrentCharacterID(string characterName)
    {
        for (int i = 0; i < characterData.Count; i++)
        {
            if (i != 0 && characterData[i].characterName == characterName)
            {
                currentCharacterID = characterData[i].characterID;
                break;
            }
        }
    }
    public void SetAttackElementType(ElementType elementType)
    {
        attackData[currentCharacterID].elementType = elementType;
    }
    public void SetInvincible(bool active )
    {
        isInvincible = active;
    }
    public void StartCountIsInvincibleTime(float time)
    {
        StartCoroutine(CountIsInvincibleTime(time));
    }
    private IEnumerator CountIsInvincibleTime(float time)
    {
        isInvincible = true;
        yield return Yielders.GetWaitForSeconds(time);
        isInvincible = false;
    }

    public bool GetPlayerCanStun()
    {
        return playerUnit.canStun;
    }
}
