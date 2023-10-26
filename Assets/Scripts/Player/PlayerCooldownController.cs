using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

/// <summary>
/// CD計算 完全在這裡計算
/// </summary>
public class PlayerCooldownController : SerializedMonoBehaviour
{
    [HideInInspector] public Action DodgeCooldownTrigger;
    [SerializeField]private Dictionary<string ,float> dodgeCooldownTimeDic = new Dictionary<string, float>();
    [SerializeField] private Dictionary<string, Coroutine> dodgeCooldownCorDic = new Dictionary<string, Coroutine>();

    [HideInInspector] public Action<string> Skill1CooldownTrigger;
    [HideInInspector] public Action<string> Skill2CooldownTrigger;
    [HideInInspector] public Action<string> USkillCooldownTrigger; //各角色的CD觸發
    [HideInInspector] public UnityAction moUSkillTriggerEvent;  //各角色觸發大招時執行事件(播放動畫 特效等)
    [HideInInspector] public UnityAction liaUSkillTriggerEvent;

    [HideInInspector] public Action<string> CharacterSwitchCooldownTrigger;

    private Action<string> DodgeCooldownEndAction;

    public float moSkill1CurrentCoolDown;
    public List<float> moSkill2CurrentCoolDowns;
    public float moUSkillCurrentCoolDown;
    public float liaSkill1CurrentCoolDown;
    public float liaSkill2CurrentCoolDown;
    public float liaUSkillCurrentCoolDown;

    public float characterSwitchCoolDown;

    //各角色技能腳本
    public MoSkill1Attack moSkill1;
    public MoSkill2Attack moSkill2;
    public MoUSkillEffectSpawner moUSkill;
    public LiaSkill1Spawner liaSkill1;
    public LiaSkill2Spawner liaSkill2;

    private PlayerInput playerInput;
    private PlayerCharacterStats characterStats;

    public AttackButtons attackButtons;
    public CharacterSwitchButtons characterSwitchButtons;
    //private Coroutine dodgeCor;

    private void OnEnable()
    {
        DodgeCooldownTrigger += DodgeCooldownStart;
        Skill1CooldownTrigger += Skill1CooldownStart;
        Skill2CooldownTrigger += Skill2CooldownStart;
        USkillCooldownTrigger += USkillCooldownStart;

        CharacterSwitchCooldownTrigger += CharacterSwitchCooldownStart;

        DodgeCooldownEndAction += ClearDodgeCor;
    }
    private void OnDisable()
    {
        DodgeCooldownTrigger -= DodgeCooldownStart;
        Skill1CooldownTrigger -= Skill1CooldownStart;
        Skill2CooldownTrigger -= Skill2CooldownStart;
        USkillCooldownTrigger -= USkillCooldownStart;

        CharacterSwitchCooldownTrigger -= CharacterSwitchCooldownStart;

        DodgeCooldownEndAction -= ClearDodgeCor;
        StopAllCoroutines();
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        characterStats = GetComponent<PlayerCharacterStats>();
    }
    private void DodgeCooldownStart()
    {
        if (dodgeCooldownCorDic[characterStats.characterData[characterStats.currentCharacterID].characterName] == null)
        {
            Coroutine dodgeCor = StartCoroutine(DodgeCooldown());
            dodgeCooldownCorDic[characterStats.characterData[characterStats.currentCharacterID].characterName] = dodgeCor;
        }
    }
    private IEnumerator DodgeCooldown()
    {
        int currentID = characterStats.currentCharacterID;
        playerInput.canDodge[currentID] = false;
        yield return Yielders.GetWaitForSeconds(dodgeCooldownTimeDic[characterStats.characterData[currentID].characterName]);
        playerInput.canDodge[currentID] = true;

        DodgeCooldownEndAction?.Invoke(characterStats.characterData[currentID].characterName);
    }
    private void ClearDodgeCor(string characterName)
    {
        if (dodgeCooldownCorDic[characterName] != null)
        {
            StopCoroutine(dodgeCooldownCorDic[characterName]);
            dodgeCooldownCorDic[characterName] = null;
        }
    }

    private void CharacterSwitchCooldownStart(string characterName)
    {
        StartCharacterSwitchCoolDown();
        characterSwitchButtons.StartSwitchCooldown(characterName);
    }
    public void StartCharacterSwitchCoolDown()
    {
        StartCoroutine(CharacterSwitchCoolDown());
    }

    /// <summary>
    /// 切換角色CD計算
    /// </summary>
    private IEnumerator CharacterSwitchCoolDown()
    {
        for (int i = 0; i < playerInput.canCharacterSwitch.Count; i++)
        {
            playerInput.canCharacterSwitch[i] = false;
        }
        yield return Yielders.GetWaitForSeconds(characterSwitchButtons.switchCooldownTime);
        for (int i = 0; i < playerInput.canCharacterSwitch.Count; i++)
        {
            playerInput.canCharacterSwitch[i] = true;
        }
    }

    #region 技能CD
    private void Skill1CooldownStart(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                StartMoSkill1CoolDown();
                attackButtons.StartSkill1Count(characterName);
                break;
            case "Lia":
                StartLiaSkill1CoolDown();
                attackButtons.StartSkill1Count(characterName);
                break;
        }
    }
    private void Skill2CooldownStart(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                StartMoSkill2CoolDown();
                attackButtons.StartSkill2Count(characterName);
                break;
            case "Lia":
                StartLiaSkill2CoolDown();
                attackButtons.StartSkill2Count(characterName);
                break;
        }
    }
    private void USkillCooldownStart(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                moUSkillTriggerEvent?.Invoke();
                Live2DAnimationManager.Instance.StartMoUSkillAnimation();
                StartMoUSkillCoolDown();
                attackButtons.StartUSkillCount(characterName);
                break;
                //case "Lia":
                //    StartLiaUSkillCoolDown();
                //    attackButtons.StartUSkillCount(characterName);
                //    break;
        }
    }
    #region Mo技能CD執行
    public void StartMoSkill1CoolDown()
    {
        StartCoroutine(MoSkill1CoolDown());
        StartCoroutine(MoSkill1CoolDownCount());
    }
    private IEnumerator MoSkill1CoolDownCount()
    {
        moSkill1CurrentCoolDown = 0;
        int CharacterID = moSkill1.characterStats.currentCharacterID;
        while (!playerInput.canSkill1[CharacterID])
        {
            moSkill1CurrentCoolDown += Time.deltaTime;
            yield return null;
        }
        moSkill1CurrentCoolDown = 0;
    }
    private IEnumerator MoSkill1CoolDown()
    {
        int CharacterID = moSkill1.characterStats.currentCharacterID;
        playerInput.canSkill1[CharacterID] = false;
        yield return Yielders.GetWaitForSeconds(moSkill1.skillData.skillCoolDown);
        playerInput.canSkill1[CharacterID] = true;
    }

    public void StartMoSkill2CoolDown()
    {
        if (moSkill2.currentUses < moSkill2.maxUses)
        {
            moSkill2.StartSpawnEffect();
            moSkill2.currentUses++;
            attackButtons.Skill2CountIconRemove();
            if (moSkill2.currentUses == 1)
            {
                StartCoroutine(MoSkill2TimeCount());
                StartCoroutine(MoSkill2lCoolDown(1));
                StartCoroutine(MoSkill2CoolDownCount());
            }
            else if (moSkill2.currentUses == 2)
            {
                StartCoroutine(MoSkill2lCoolDown(2 - moSkill2.tempTimeCount));

                StartCoroutine(MoSkill2CoolDownCount());
            }
        }
    }
    private IEnumerator MoSkill2CoolDownCount()
    {
        int CharacterID = moSkill2.characterStats.currentCharacterID;
        if (moSkill2.currentUses == 1)
        {
            moSkill2CurrentCoolDowns[0] = 0;
            while (moSkill2CurrentCoolDowns[0] <= moSkill2.skillData.skillCoolDown)
            {
                moSkill2CurrentCoolDowns[0] += Time.deltaTime;
                yield return null;
            }
            moSkill2CurrentCoolDowns[0] = 0;
        }
        else if (moSkill2.currentUses == 2)
        {
            moSkill2CurrentCoolDowns[1] = 0;
            while (moSkill2CurrentCoolDowns[1] <= moSkill2.skillData.skillCoolDown)
            {
                moSkill2CurrentCoolDowns[1] += Time.deltaTime;
                yield return null;
            }
            moSkill2CurrentCoolDowns[1] = 0;
        }
    }
    private IEnumerator MoSkill2TimeCount()
    {
        moSkill2.tempTimeCount = 0;
        while (moSkill2.currentUses != 2 && moSkill2.tempTimeCount <= 1)
        {
            if (moSkill2.tempTimeCount <= 1)
            {
                moSkill2.tempTimeCount += Time.deltaTime;
            }
            yield return null;
        }
    }
    private IEnumerator MoSkill2lCoolDown(float timeMul)
    {
        if (moSkill2.currentUses >= 2)
        {
            playerInput.canSkill2[moSkill2.characterStats.currentCharacterID] = false;
        }

        yield return Yielders.GetWaitForSeconds(moSkill2.skillData.skillCoolDown * timeMul);
        if (moSkill2.currentUses > 0)
        {
            moSkill2.currentUses -= 1;
            attackButtons.Skill2CountIconAdd(moSkill2.characterStats.characterData[moSkill2.characterStats.currentCharacterID].characterName);
            playerInput.canSkill2[moSkill2.characterStats.currentCharacterID] = true;
        }
    }

    public void StartMoUSkillCoolDown()
    {
        StartCoroutine(MoUSkillCoolDown());
        StartCoroutine(MoUSkillCoolDownCount());
    }
    private IEnumerator MoUSkillCoolDownCount()
    {
        moUSkillCurrentCoolDown = 0;
        moUSkill.characterStats.CurrnetUSkillValue = 0;
        int CharacterID = moUSkill.characterStats.currentCharacterID;
        while (!playerInput.canUSkill[CharacterID])
        {
            moUSkillCurrentCoolDown += Time.deltaTime;
            yield return null;
        }
        moUSkillCurrentCoolDown = 0;
    }
    private IEnumerator MoUSkillCoolDown()
    {
        int CharacterID = moUSkill.characterStats.currentCharacterID;
        playerInput.canUSkill[CharacterID] = false;
        yield return Yielders.GetWaitForSeconds(moUSkill.skillData.skillCoolDown);
        playerInput.canUSkill[CharacterID] = true;
    }
    #endregion
    #region Lia技能CD執行
    public void StartLiaSkill1CoolDown()
    {
        StartCoroutine(LiaSkill1CoolDown());
        StartCoroutine(LiaSkill1CoolDownCount());
    }
    private IEnumerator LiaSkill1CoolDown()
    {
        int CharacterID = liaSkill1.characterStats.currentCharacterID;
        playerInput.canSkill1[CharacterID] = false;
        yield return Yielders.GetWaitForSeconds(liaSkill1.skillData.skillCoolDown);
        playerInput.canSkill1[CharacterID] = true;
    }
    private IEnumerator LiaSkill1CoolDownCount()
    {
        liaSkill1CurrentCoolDown = 0;
        int CharacterID = liaSkill1.characterStats.currentCharacterID;
        while (!playerInput.canSkill1[CharacterID])
        {
            liaSkill1CurrentCoolDown += Time.deltaTime;
            yield return null;
        }
        liaSkill1CurrentCoolDown = 0;
    }
    public void StartLiaSkill2CoolDown()
    {
        StartCoroutine(LiaSkill2CoolDown());
        StartCoroutine(LiaSkill2CoolDownCount());
    }
    private IEnumerator LiaSkill2CoolDown()
    {
        int CharacterID = liaSkill2.characterStats.currentCharacterID;
        playerInput.canSkill2[CharacterID] = false;
        yield return Yielders.GetWaitForSeconds(liaSkill2.skillData.skillCoolDown);
        playerInput.canSkill2[CharacterID] = true;
    }
    private IEnumerator LiaSkill2CoolDownCount()
    {
        liaSkill2CurrentCoolDown = 0;
        int CharacterID = liaSkill2.characterStats.currentCharacterID;
        while (!playerInput.canSkill2[CharacterID])
        {
            liaSkill2CurrentCoolDown += Time.deltaTime;
            yield return null;
        }
        liaSkill2CurrentCoolDown = 0;
    }
    #endregion
    #endregion
}
