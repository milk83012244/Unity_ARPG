using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

/// <summary>
/// 獲取角色資料 可造成傷害(可切換角色)
/// </summary>
public class PlayerCharacterStats : SerializedMonoBehaviour
{
    private PlayerUnit playerUnit;
    public AttackButtons attackButtons;
    public CharacterSwitchButtons switchButtons;

    public int currentCharacterID;

    //之後要改成隊伍有對應角色重新載入成對應角色的資料(用字典等鍵值對儲存)
    public List<CharacterBattleDataSO> characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO> attackData = new List<CharacterAttackDataSO>();
    public List<CharacterElementCountSO> elementCountData = new List<CharacterElementCountSO>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    [HideInInspector] public bool canDownAttack =true; //共用攻擊力倍率:傷害計算完後才加算
    private float attackDownRate = 1;
    public float AttackDownRate //攻擊下降倍率 最低0.5倍
    {
        get
        {
            return attackDownRate;
        }
        set
        {
            if (canDownAttack && attackDownRate >= 0.5f) //可被降低攻擊
                attackDownRate = value;
            else if (attackDownRate <= 0.5f)
                attackDownRate = 0.5f;
            else
                attackDownRate = 1;
        }
    }
    private float attackRaiseRate = 1;
    public float AttackRaiseRate //攻擊上升倍率
    {
        get
        {
            return attackRaiseRate;
        }
        set
        {
            if ( attackRaiseRate >= 4f) //提升上限
                attackRaiseRate = 4;
            else if (attackRaiseRate < 1)
                attackRaiseRate = 1;
            else
                attackRaiseRate = value;
        }
    }

    public bool isInvincible;

    public Action<float> stunValueChangedAction;
    public Action<float> stunValueIsMaxAction;
    public UnityAction USkillValueMaxAction; //必殺技槽滿時觸發

    [HideInInspector] public UnityAction hpZeroEvent; //當前角色HP歸0事件

    Coroutine StunValueCountCor;

    #region 從ChatacterDataSO讀取值
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

            if (!characterData[currentCharacterID].isDown && characterData[currentCharacterID].currentHealth <= 0) //HP歸零時
            {
                characterData[currentCharacterID].currentHealth = 0;
                characterData[currentCharacterID].isDown = true;
                switchButtons.SetCharacterHPActive(currentCharacterID, false);
                hpZeroEvent?.Invoke();
            }
            else if (characterData[currentCharacterID].isDown && characterData[currentCharacterID].currentHealth > 0) //復活時
            {
                switchButtons.SetCharacterHPActive(currentCharacterID, true);
            }
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
                Debug.Log(string.Format("<color=red>{0}</color>", characterData[currentCharacterID].characterName + "被觸發硬直"));
                //進入硬直狀態
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
                Debug.Log(string.Format("<color=red>{0}</color>", characterData[currentCharacterID].characterName + "可以使用必殺技"));
                USkillValueMaxAction?.Invoke();
            }
        }
    }

    #endregion
    private void OnEnable()
    {
        stunValueChangedAction += StartStunCount;

        for (int i = 0; i < characterData.Count; i++) //重置硬直值
        {
            characterData[i].currentStunValue = 0;
        }
    }
    private void OnDestroy()
    {
        stunValueChangedAction -= StartStunCount;
    }
    private void Start()
    {
        playerUnit = GetComponent<PlayerUnit>();
    }

    #region 傷害計算
    /// <summary>
    /// 造成傷害(獲得敵方腳本扣生命值)
    /// freeMul = 獨立倍率例如技能1,2倍率以外的單獨倍率傷害
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false, bool isCounter = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "是無敵狀態 傷害無效"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, isCritical, isSkill1, isSkill2, isCounter, freeMul) - defender.CurrentDefence, 0);

        damagefloat *= attackRaiseRate *= AttackDownRate;
        currentDamage = (int)Mathf.Round(damagefloat);

        #region 調試

        if (isSkill1)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "使用技能1對" + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        else if (isSkill2)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 使用技能2對 " + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        else if (isCounter)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "使用反擊對" + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "額外倍率對" + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 使用一般/持續攻擊對 " + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        #endregion

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
        //CinemachineShake.GetInstance().ShakeCamera(0.3f, 0.1f);//造成傷害時攝影機震動
    }
    /// <summary>
    /// 標記傷害
    /// </summary>
    public void TakeMarkDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "是無敵狀態 傷害無效"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateMarkDamage(attacker, isCritical, freeMul) - (defender.CurrentDefence), 0);

        damagefloat *= attackRaiseRate *= AttackDownRate;
        currentDamage = (int)Mathf.Round(damagefloat);

        if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "額外倍率對" + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 的標記對 " + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 副傷害
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isCritical = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "是無敵狀態 傷害無效"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();

        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, attackData[currentCharacterID].subMultplier, isCritical, freeMul) - (defender.CurrentDefence), 0);

        damagefloat *= attackRaiseRate *= AttackDownRate;
        currentDamage = (int)Mathf.Round(damagefloat);

        if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "額外倍率對" + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 的副攻擊對 " + defender.enemyBattleData.characterName + "的傷害: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 造成屬性傷害(獲得敵方腳本扣生命值)
    /// </summary>
    public void TakeDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false, bool isSkill1 = false, bool isSkill2 = false,bool isStatus2 = false, bool isStatusMix = false, float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "是無敵狀態 傷害無效"));
            return;
        }

        //獲得計算器
        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //扣除防禦力
        float damagefloat = Mathf.Max(damageCalculator.CalculateDamage(attacker, elementType, isCritical, isSkill1, isSkill2, freeMul) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        damagefloat *= attackRaiseRate *= AttackDownRate;
        currentDamage = (int)Mathf.Round(damagefloat);
        #region 調試
        if (isSkill1)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "使用技能1對" + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else if (isSkill2)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 使用技能2對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "額外倍率對" + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else if (isStatus2)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 的屬性2階對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else if (isStatusMix)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 觸發屬性混合對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 使用一般/持續攻擊對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        #endregion

        //造成傷害
        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    /// <summary>
    /// 屬性副傷害
    /// </summary>
    public void TakeSubDamage(PlayerCharacterStats attacker, OtherCharacterStats defender, ElementType elementType, bool isCritical = false, bool isSkill = false, string FromOthersName = "", float freeMul = 0)
    {
        if (defender.isInvincible)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", defender.enemyBattleData.characterName + "是無敵狀態 傷害無效"));
            return;
        }

        DamageCalculator damageCalculator = new DamageCalculator();
        float currentDefence = defender.CurrentDefence;

        //扣除防禦力
        float damagefloat = Mathf.Max(damageCalculator.CalculateSubDamage(attacker, attackData[currentCharacterID].subMultplier, elementType) -
            (defender.CurrentDefence + (currentDefence *= defender.GetElementDefence(elementType))), 0);

        damagefloat *= attackRaiseRate *= AttackDownRate;
        currentDamage = (int)Mathf.Round(damagefloat);
        if (FromOthersName != "")
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", FromOthersName + "的附加傷害對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else if (freeMul != 0)
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + "額外倍率對" + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }
        else
        {
            Debug.Log(string.Format("<color=yellow>{0}</color>", characterData[currentCharacterID].characterName + " 的副屬性攻擊對 " + defender.enemyBattleData.characterName + "造成" + elementType.ToString() + "屬性的傷害: " + currentDamage));
        }

        defender.CurrnetHealth = defender.CurrnetHealth - currentDamage;

        //UI更新
        //經驗提升等
    }
    public void TakeStunValue(PlayerCharacterStats attacker, OtherCharacterStats defender, bool isSkill1 = false, bool isSkill2 = false, float freeMul = 0)
    {
        float defenderStunValue = defender.CurrnetStunValue;
        float attackStunValue = attacker.attackData[currentCharacterID].stunValue;
        if (isSkill1)
        {
            attackStunValue *= attackData[currentCharacterID].skill1StunValueMultplier - defender.enemyBattleData.stunResistance;
        }
        else if (isSkill2)
        {
            attackStunValue *= attackData[currentCharacterID].skill2StunValueMultplier - defender.enemyBattleData.stunResistance;
        }
        else if (freeMul != 0)
        {
            attackStunValue  *= freeMul - defender.enemyBattleData.stunResistance;
        }
        else
        {
            attackStunValue  -= defender.enemyBattleData.stunResistance;
        }

        defenderStunValue += attackStunValue;
        defender.CurrnetStunValue = defenderStunValue;
    }

    /// <summary>
    /// 啟動遞減硬值值
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
                Debug.Log("目前硬直值: " + tempStunValue);

            // Debug.Log("目前硬直值: " + characterData[currentCharacterID].currentStunValue);
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
    /// 初始化角色數值
    /// </summary>
    public void InitData()
    {

    }
    /// <summary>
    /// 切換角色時重設數值
    /// </summary>
    public void SwitchReset()
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
    public void SetInvincible(bool active)
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
    public void SetAttackRate(bool isRaise,float rate)
    {
        if (isRaise)
        {
            attackRaiseRate += rate;
        }
        else
        {
            attackDownRate -= rate;
        }
    }

    public bool GetPlayerCanStun()
    {
        return playerUnit.canStun;
    }
}
