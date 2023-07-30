using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum EnemyCurrentState
{
    Idle,
    Chase,
    Patrol,
    Attack,
    Stunning, //�w�� 
    Stop, //�L�k��ʪ��A(�w�����ಾ�ʪ����A)
    Dead,
}

/// <summary>
/// ��������ƨíp�� 
/// </summary>
public class OtherCharacterStats : MonoBehaviour
{
    private ElementStatusEffect elementStatus;
    [HideInInspector]public CharacterElementCounter characterElementCounter;

    public CharacterBattleDataSO baseCharacterData;
    public CharacterAttackDataSO baseAttackData;
    public CharacterElementCountSO baseElementCountData;
    public CharacterRewardDataSO characterReward;

    public CharacterBattleDataSO enemyBattleData;
    public CharacterAttackDataSO enemyAttackData;
    public CharacterElementCountSO enemyElementCountData;
    public Transform elementCountParent;

    public List<float> ElementDefences = new List<float>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    public bool isInvincible;
    public bool isDeadEventEnd = false;

    public Action<float> stunValueChangedAction;
    public Action<float> stunValueIsMaxAction;
    public Action<ElementType> elementStates2Action; //�ݩʪ��A2Ĳ�o�ƥ�

    [HideInInspector] public UnityEvent hpZeroEvent;

    Coroutine StunValueCountCor;

    #region �qChatacterDataSOŪ����
    public int MaxHealth
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.mexHealth;
            else
                return 0;
        }
        set
        {
            enemyBattleData.mexHealth = value;
        }
    }
    public int CurrnetHealth
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.currentHealth;
            else
                return 0;
        }
        set
        {
            enemyBattleData.currentHealth = value;
            if (enemyBattleData.currentHealth <= 0)
            {
                if (!isDeadEventEnd)
                {
                    hpZeroEvent?.Invoke();
                    RewardManager.Instance.onEnemyDefeatedEvent?.Invoke(characterReward);
                    isDeadEventEnd = true;
                }
            }
        }
    }
    public float BaseDefence
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.baseDefence;
            else
                return 0;
        }
        set
        {
            enemyBattleData.baseDefence = value;
        }
    }
    public float CurrentDefence
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.currentDefence;
            else
                return 0;
        }
        set
        {
            enemyBattleData.currentDefence = value;
        }
    }
    public float MaxStunValue
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.maxStunValue;
            else
                return 0;
        }
        set
        {
            enemyBattleData.maxStunValue = value;
        }
    }
    public float CurrnetStunValue
    {
        get
        {
            if (enemyBattleData != null)
                return enemyBattleData.currentStunValue;
            else
                return 0;
        }
        set
        {
            enemyBattleData.currentStunValue = value;
            StopStunCount();
            if (enemyBattleData.currentStunValue >=0 && enemyBattleData.currentStunValue< enemyBattleData.maxStunValue)
                stunValueChangedAction?.Invoke(enemyBattleData.stunValueTime);

            if (enemyBattleData.currentStunValue >= enemyBattleData.maxStunValue)
            {
                enemyBattleData.currentStunValue = enemyBattleData.maxStunValue;
                Debug.Log(string.Format("<color=red>{0}</color>", enemyBattleData.characterName + "Ĳ�o�w��"));
                //�i�J�w�����A
                stunValueIsMaxAction?.Invoke(enemyBattleData.stunRecovorTime);
            }
        }
    }
    #endregion

    private void OnEnable()
    {
        enemyElementCountData.ResetAllData();

        stunValueChangedAction += StartStunCount;
    }
    private void OnDestroy()
    {
        enemyElementCountData.addElementCountEvent.RemoveListener(SetCurrentElementStatusUIShow);
        enemyElementCountData.removeElementCountEvent.RemoveListener(SetCurrentElementStatusUIHide);

        stunValueChangedAction -= StartStunCount;
    }
    private void Awake()
    {
        elementStatus = GetComponentInChildren<ElementStatusEffect>();
        characterElementCounter = GetComponent<CharacterElementCounter>();

         enemyBattleData = Instantiate(baseCharacterData);
         enemyAttackData = Instantiate(baseAttackData);
         enemyElementCountData = Instantiate(baseElementCountData);
    }
    private void Start()
    {
        enemyElementCountData.addElementCountEvent.AddListener(SetCurrentElementStatusUIShow); //�ᤩ�ݩʨƥ��ť
        enemyElementCountData.removeElementCountEvent.AddListener(SetCurrentElementStatusUIHide); //�����ݩʨƥ��ť
    }

    #region �ˮ`�p��
    /// <summary>
    /// �ĤH�y���ˮ`
    /// </summary>
    public void TakeDamage(OtherCharacterStats attacker, PlayerCharacterStats defender, bool isCritical = false, bool isSkill = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.characterData[defender.currentCharacterID].characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, isCritical, isSkill, freeMul) - (defender.CurrentDefence), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        Debug.Log(string.Format("<color=yellow>{0}</color>", attacker.enemyBattleData.characterName + " �ϥΤ@������� " + defender.characterData[defender.currentCharacterID].characterName + "�y���ˮ`: " + currentDamage));
        //Debug.Log(string.Format("<color=yellow>{0}</color>", characterData.characterName + "�p��᪺�ˮ`: " + currentDamage));

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.1f);//�y���ˮ`����v���_��
    }
    /// <summary>
    /// �y���ݩʶˮ`
    /// </summary>
    public void TakeDamage(OtherCharacterStats attacker, PlayerCharacterStats defender, ElementType elementType, bool isCritical = false, bool isSkill = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.characterData[defender.currentCharacterID].characterName + "�O�L�Ī��A �ˮ`�L��"));
            return;
        }

        //��o�p�⾹
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //�������m�O
        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType,freeMul: freeMul) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        currentDamage = (int)Mathf.Round(damagefloat);

        Debug.Log(string.Format("<color=yellow>{0}</color>", attacker.enemyBattleData.characterName + " �ϥΤ@������� " + defender.characterData[defender.currentCharacterID].characterName + "�y��" + elementType.ToString() + "�ݩʪ��ˮ`: " + currentDamage));
        //Debug.Log(string.Format("<color=yellow>{0}</color>", characterData.characterName + "�p��᪺�ݩʶˮ`: " + currentDamage));

        //�y���ˮ`
        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

    }

    public void TakeStunValue(OtherCharacterStats attacker, PlayerCharacterStats defender, bool isSkill = false, float freeMul = 0)
    {
        float defenderStunValue = defender.CurrnetStunValue;
        float attackStunValue;
        if (isSkill)
        {
            attackStunValue = attacker.enemyAttackData.stunValue *= attacker.enemyAttackData .skill1Multplier- defender.characterData[defender.currentCharacterID].stunResistance;
        }
        else if (freeMul != 0)
        {
            attackStunValue = attacker.enemyAttackData.stunValue *= freeMul - defender.characterData[defender.currentCharacterID].stunResistance;
        }
        else
        {
            attackStunValue = attacker.enemyAttackData.stunValue - defender.characterData[defender.currentCharacterID].stunResistance;
        }

        defenderStunValue += attackStunValue;
        defender.CurrnetStunValue = defenderStunValue;
    }
    #endregion
    public float GetElementDefence(ElementType elementType)
    {
        return enemyBattleData.elementDefense[elementType];
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
        while (enemyBattleData.currentStunValue > 0)
        {
            yield return null;
            timer += Time.deltaTime;
            float t = timer / time;
            enemyBattleData.currentStunValue = Mathf.Lerp(MaxStunValue, 0, t);
            int tempStunValue = (int)(enemyBattleData.currentStunValue / 1);

            if (tempStunValue % 10 == 0)
                Debug.Log("�ثe�w����: " + tempStunValue);
        }
        yield break;
    }
    public void StopStunCount()
    {
        if (StunValueCountCor != null)
        {
            StopCoroutine(StunValueCountCor);
            StunValueCountCor = null;
        }
    }

    /// <summary>
    /// �]�w�ۨ��{�b���ݩʽᤩ���A
    /// </summary>
    public void SetCurrentElementStatusUIShow()
    {
        foreach (KeyValuePair<ElementType,int> item in enemyElementCountData.elementCountDic)
        {
            switch (item.Key)
            {
                case ElementType.Fire:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Fire").gameObject.SetActive(true);
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "���U�N�ĪG");
                        elementCountParent.Find("Fire").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Ice:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Ice").gameObject.SetActive(true);
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "���B��ĪG");
                        elementCountParent.Find("Ice").gameObject.SetActive(false);
                        elementStatus.SetElementActive(ElementType.Ice, true);
                        elementStates2Action?.Invoke(ElementType.Ice);
                    }
                    break;
                case ElementType.Wind:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Wind").gameObject.SetActive(true);
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "�������ĪG");
                        elementCountParent.Find("Wind").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Thunder:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Thunder").gameObject.SetActive(true);
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "���·��ĪG");
                        elementCountParent.Find("Thunder").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Light:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Light").gameObject.SetActive(true);
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "�����z�ĪG");
                        elementCountParent.Find("Light").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Dark:
                    if (item.Value == 1)
                    {
                        elementCountParent.Find("Dark").gameObject.SetActive(true); ;
                    }
                    else if (item.Value == 2)
                    {
                        Debug.Log("Ĳ�o" + item.Key + "���t�z�ĪG");
                        elementCountParent.Find("Dark").gameObject.SetActive(false);
                    }
                    break;
                default:
                    Debug.Log("�S���ᤩ�ݩ�");
                    break;
            }
        }
    }

    public void SetCurrentElementStatusUIHide()
    {
        foreach (KeyValuePair<ElementType, int> item in enemyElementCountData.elementCountDic)
        {
            switch (item.Key)
            {
                case ElementType.Fire:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Fire").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Ice:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Ice").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Wind:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Wind").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Thunder:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Thunder").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Light:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Light").gameObject.SetActive(false);
                    }
                    break;
                case ElementType.Dark:
                    if (item.Value == 0)
                    {
                        elementCountParent.Find("Dark").gameObject.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
