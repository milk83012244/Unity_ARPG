using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using System;
using Sirenix.Utilities;

public class AttackButtons : SerializedMonoBehaviour
{
    public PlayerCooldownController cooldownController;
    public PlayerCharacterSwitch playerCharacterSwitch;

    public Dictionary<string, SkillDataSO> skill1List = new Dictionary<string, SkillDataSO>();
    public Dictionary<string, SkillDataSO> skill2List = new Dictionary<string, SkillDataSO>();
    public Dictionary<string, SkillDataSO> USkillList = new Dictionary<string, SkillDataSO>();

    public Dictionary<string, Sprite> normalAttackImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> skill1SlotImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> skill2SlotImages = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> USkillSlotImages = new Dictionary<string, Sprite>();

    public Dictionary<ElementType, Sprite> LiaNormalAttackSlotElementImages = new Dictionary<ElementType, Sprite>();
    public Dictionary<ElementType, Sprite> LiaSkill1SlotElementImages = new Dictionary<ElementType, Sprite>();
    public Dictionary<ElementType, Sprite> LiaSkill2SlotElementImages = new Dictionary<ElementType, Sprite>();
    public Dictionary<ElementType, Sprite> LiaUSkillSlotElementImages = new Dictionary<ElementType, Sprite>();

    //мicon
    public Image skill1IconCooldownMask;
    public Image skill2IconCooldownMask;
    public Image USkillIconCooldownMask;
    public Image USkillIconAddBar;

    public Image normalAttackIconSlot;
    public Image skill1IconSlot;
    public Image skill2IconSlot;
    public Image USkillIconSlot;

    public GameObject normalAttackUnUseMask;
    public GameObject skill1UnUseMask;
    public GameObject skill2UnUseMask;
    public GameObject USkillUnUseMask;

    public List<Image> skill1CountIcon = new List<Image>();
    public GameObject skill2CountIconParent;
    public List<Image> skill2CountIcon = new List<Image>();

    //硈尿ㄏノмΩ计icon陪ボノ
    private Stack<Image> ActiveSkill1CountIcon = new Stack<Image>();
    private Stack<Image> ActiveSkill2CountIcon = new Stack<Image>();
    private Stack<Image> ActiveSkill1CountIconTemp = new Stack<Image>();
    private Stack<Image> ActiveSkill2CountIconTemp = new Stack<Image>();

    //玱丁挡ㄆン
    [HideInInspector] public Action<string> Skill1CountCooldownTrigger;
    [HideInInspector] public Action<string> Skill2CountCooldownTrigger;

    //秙琌ノㄆン
    [HideInInspector] public Action<bool> normalAttackCanUseAction;
    [HideInInspector] public Action<bool> skill1CanUseAction;
    [HideInInspector] public Action<bool> skill2CanUseAction;
    [HideInInspector] public Action<bool> USkillCanUseAction;

    //CD璸ゅ
    //м
    public List<TextMeshProUGUI> skill1CooldownTexts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> skill2CooldownTexts = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> USkillCooldownTexts = new List<TextMeshProUGUI>();

    private Coroutine skill1CooldownCor;
    private Coroutine skill2CooldownCor;
    private Coroutine USkillCooldownCor;

    private void OnEnable()
    {
        Skill1CountCooldownTrigger += Skill1CountCheck;
        Skill2CountCooldownTrigger += Skill2CountCheck;

        normalAttackCanUseAction += SetNormalAttackButtonActive;
        skill1CanUseAction += SetSkill1ButtonActive;
        skill2CanUseAction += SetSkill2ButtonActive;
        USkillCanUseAction += SetUSkillButtonActive;

        playerCharacterSwitch.onCharacterSwitch += SetSkillIcon;
        playerCharacterSwitch.onCharacterSwitch += SetSkillCountIcon;
        playerCharacterSwitch.onNormalToBattleMode += SetSkillIcon;
        playerCharacterSwitch.onNormalToBattleMode += SetSkillCountIcon;
    }
    private void OnDisable()
    {
        Skill1CountCooldownTrigger -= Skill1CountCheck;
        Skill2CountCooldownTrigger -= Skill2CountCheck;

        normalAttackCanUseAction -= SetNormalAttackButtonActive;
        skill1CanUseAction -= SetSkill1ButtonActive;
        skill2CanUseAction -= SetSkill2ButtonActive;
        USkillCanUseAction -= SetUSkillButtonActive;

        playerCharacterSwitch.onCharacterSwitch -= SetSkillIcon;
        playerCharacterSwitch.onCharacterSwitch += SetSkillCountIcon;
        playerCharacterSwitch.onNormalToBattleMode -= SetSkillIcon;
        playerCharacterSwitch.onNormalToBattleMode -= SetSkillCountIcon;

        StopAllCoroutines();
    }
    public void SetNormalAttackButtonActive(bool active)
    {
        if (active == true)
            normalAttackUnUseMask.SetActive(false);
        else
            normalAttackUnUseMask.SetActive(true);
    }
    /// <summary>
    /// м1窽ノ瓜秨闽
    /// </summary>
    public void SetSkill1ButtonActive(bool active)
    {
        if (active ==true)
            skill1UnUseMask.SetActive(false);
        else
            skill1UnUseMask.SetActive(true);
    }
    public void SetSkill2ButtonActive(bool active)
    {
        if (active == true)
            skill2UnUseMask.SetActive(false);
        else
            skill2UnUseMask.SetActive(true);
    }
    public void SetUSkillButtonActive(bool active)
    {
        if (active == true)
            USkillUnUseMask.SetActive(false);
        else
            USkillUnUseMask.SetActive(true);
    }
    public void StartSkill1Count(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                skill1CooldownTexts[1].enabled = true;
                break;
            case "Lia":
                skill1CooldownTexts[2].enabled = true;
                break;
        }
        skill1IconCooldownMask.enabled = true;
        skill1CooldownCor = StartCoroutine(skill1CDCount(characterName));
    }
    private void Skill1CountCheck(string characterName)
    {
        if (skill1CooldownCor != null)
        {
            StopCoroutine(skill1CooldownCor);
            skill1CooldownCor = null;
        }

        if (ActiveSkill1CountIcon.Count < skill1List[characterName].skillUseCount - 1)
        {
            StartSkill1Count(characterName);
        }
        else
        {
            skill1IconCooldownMask.enabled = false;
            for (int i = 0; i < skill1CooldownTexts.Count; i++)
            {
                skill1CooldownTexts[i].enabled = false;
            }
            return;
        }
    }
    private IEnumerator skill1CDCount(string characterName)
    {
        float currentTime = skill1List[characterName].skillCoolDown;

        if (skill1List[characterName].skillUseCount > 0)
        {
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                switch (characterName)
                {
                    case "Mo":
                        skill1CooldownTexts[1].text = currentTime.ToString("F1");
                        skill1IconCooldownMask.fillAmount = currentTime / skill1List[characterName].skillCoolDown;
                        break;
                    case "Lia":
                        skill1CooldownTexts[2].text = currentTime.ToString("F1");
                        skill1IconCooldownMask.fillAmount = currentTime / skill1List[characterName].skillCoolDown;
                        break;
                }
                yield return null;
            }
            if (currentTime <= 0)
            {
                //Skill1CountIconAdd(characterName);
                Skill1CountCooldownTrigger.Invoke(characterName);
            }
        }
        else
        {
            while (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;
                switch (characterName)
                {
                    case "Mo":
                        skill1CooldownTexts[1].text = currentTime.ToString("F1");
                        skill1IconCooldownMask.fillAmount = currentTime / skill1List[characterName].skillCoolDown;
                        break;
                    case "Lia":
                        skill1CooldownTexts[2].text = currentTime.ToString("F1");
                        skill1IconCooldownMask.fillAmount = currentTime / skill1List[characterName].skillCoolDown;
                        break;
                }

                yield return null;
            }
            skill1IconCooldownMask.enabled = false;
            switch (characterName)
            {
                case "Mo":
                    skill1CooldownTexts[1].enabled = false;
                    break;
                case "Lia":
                    skill1CooldownTexts[2].enabled = false;
                    break;
            }
        }
    }

    public void StartSkill2Count(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                skill2CooldownTexts[1].enabled = true;
                break;
            case "Lia":
                skill2CooldownTexts[2].enabled = true;
                break;
        }

        skill2IconCooldownMask.enabled = true;
        skill2CooldownCor = StartCoroutine(skill2CDCount(characterName));
    }
    /// <summary>
    /// 硈尿ㄏノм2Ω计浪琩
    /// </summary>
    private void Skill2CountCheck(string characterName)
    {
        if (skill2CooldownCor != null)
        {
            StopCoroutine(skill2CooldownCor);
            skill2CooldownCor = null;
        }

        if (ActiveSkill2CountIcon.Count < skill2List[characterName].skillUseCount - 1)
        {
            StartSkill2Count(characterName);
        }
        else
        {
            skill2IconCooldownMask.enabled = false;
            for (int i = 0; i < skill2CooldownTexts.Count; i++)
            {
                skill2CooldownTexts[i].enabled = false;
            }
            return;
        }
    }
    private IEnumerator skill2CDCount(string characterName)
    {
        float currentTime = skill2List[characterName].skillCoolDown;

        if (skill2List[characterName].skillUseCount > 0)
        {
            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                switch (characterName)
                {
                    case "Mo":
                        skill2CooldownTexts[1].text = currentTime.ToString("F1");
                        skill2IconCooldownMask.fillAmount = currentTime / skill2List[characterName].skillCoolDown;
                        break;
                    case "Lia":
                        skill2CooldownTexts[2].text = currentTime.ToString("F1");
                        skill2IconCooldownMask.fillAmount = currentTime / skill2List[characterName].skillCoolDown;
                        break;
                }
                yield return null;
            }
            if (currentTime <= 0)
            {
                Skill2CountCooldownTrigger.Invoke(characterName);
            }
        }
        else
        {
            while (currentTime >= 0)
            {
                currentTime -= Time.deltaTime;
                switch (characterName)
                {
                    case "Mo":
                        skill2CooldownTexts[1].text = currentTime.ToString("F1");
                        skill2IconCooldownMask.fillAmount = currentTime / skill2List[characterName].skillCoolDown;
                        break;
                    case "Lia":
                        skill2CooldownTexts[2].text = currentTime.ToString("F1");
                        skill2IconCooldownMask.fillAmount = currentTime / skill2List[characterName].skillCoolDown;
                        break;
                }

                yield return null;
            }
            skill2IconCooldownMask.enabled = false;
            switch (characterName)
            {
                case "Mo":
                    skill2CooldownTexts[1].enabled = false;
                    break;
                case "Lia":
                    skill2CooldownTexts[2].enabled = false;
                    break;
            }
        }
    }

    public void StartUSkillCount(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                USkillCooldownTexts[1].enabled = true;
                break;
            case "Lia":
                USkillCooldownTexts[2].enabled = true;
                break;
        }
        USkillIconCooldownMask.enabled = true;
        USkillCooldownCor = StartCoroutine(USkillCDCount(characterName));
    }
    private IEnumerator USkillCDCount(string characterName)
    {
        float currentTime = USkillList[characterName].skillCoolDown;
        while (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            switch (characterName)
            {
                case "Mo":
                    USkillCooldownTexts[1].text = currentTime.ToString("F1");
                    USkillIconCooldownMask.fillAmount = currentTime / USkillList[characterName].skillCoolDown;
                    break;
                case "Lia":
                    USkillCooldownTexts[2].text = currentTime.ToString("F1");
                    USkillIconCooldownMask.fillAmount = currentTime / USkillList[characterName].skillCoolDown;
                    break;
            }

            yield return null;
        }
        USkillIconCooldownMask.enabled = false;
        switch (characterName)
        {
            case "Mo":
                USkillCooldownTexts[1].enabled = false;
                break;
            case "Lia":
                USkillCooldownTexts[2].enabled = false;
                break;
        }
    }
    /// <summary>
    /// 砞﹚мΤㄏノΩ计陪ボΩ计icon
    /// </summary>
    public void SetSkillCountIcon(string characterName)
    {
        ActiveSkill1CountIcon.Clear();
        ActiveSkill2CountIcon.Clear();

        if (skill1List[characterName].skillUseCount > 0)
        {
            for (int i = 0; i < skill1List[characterName].skillUseCount; i++)
            {
                skill1CountIcon[i].enabled = true;
                ActiveSkill1CountIcon.Push(skill1CountIcon[i]);
            }
        }
        else
        {
            for (int i = 0; i < skill1CountIcon.Count; i++)
            {
                skill1CountIcon[i].enabled = false;
            }
        }
        if (skill2List[characterName].skillUseCount > 0)
        {
            for (int i = 0; i < skill2List[characterName].skillUseCount; i++)
            {
                skill2CountIcon[i].enabled = true;
                ActiveSkill2CountIcon.Push(skill2CountIcon[i]);
            }
        }
        else
        {
            for (int i = 0; i < skill2CountIcon.Count; i++)
            {
                skill2CountIcon[i].enabled = false;
            }
        }
    }
    public void Skill1CountIconAdd(string characterName)
    {
        if (ActiveSkill1CountIcon.Count <= skill1List[characterName].skillUseCount)
        {
            Image countImage = ActiveSkill1CountIconTemp.Peek();
            countImage.enabled = true;
            ActiveSkill1CountIcon.Push(countImage);
            ActiveSkill1CountIconTemp.Pop();
        }
    }
    public void Skill1CountIconRemove()
    {
        if (ActiveSkill1CountIcon.Count > 0)
        {
            Image countImage = ActiveSkill1CountIcon.Peek();
            countImage.enabled = false;
            ActiveSkill1CountIconTemp.Push(countImage);
            ActiveSkill1CountIcon.Pop();
        }
    }
    public void Skill2CountIconAdd(string characterName)
    {
        if (ActiveSkill2CountIcon.Count <= skill2List[characterName].skillUseCount)
        {
            Image countImage = ActiveSkill2CountIconTemp.Peek();
            countImage.enabled = true;
            ActiveSkill2CountIcon.Push(countImage);
            ActiveSkill2CountIconTemp.Pop();
        }
    }
    public void Skill2CountIconRemove()
    {
        if (ActiveSkill2CountIcon.Count > 0)
        {
            Image countImage = ActiveSkill2CountIcon.Peek();
            countImage.enabled = false;
            ActiveSkill2CountIconTemp.Push(countImage);
            ActiveSkill2CountIcon.Pop();
        }
    }
    /// <summary>
    /// ち传à︹ち传м篈陪ボ
    /// </summary>
    public void SetSkillIcon(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                if (cooldownController.moSkill1CurrentCoolDown > 0)
                {
                    SetIconCoolDown(characterName);
                    skill1IconCooldownMask.enabled = true;
                }
                else
                {
                    for (int i = 0; i < skill1CooldownTexts.Count; i++)
                    {
                        if (i != 1)
                        {
                            skill1CooldownTexts[i].enabled = false;
                        }
                    }
                    skill1IconCooldownMask.enabled = false;
                }
                if (cooldownController.moSkill2CurrentCoolDowns[0] > 0 || cooldownController.moSkill2CurrentCoolDowns[1] > 0)
                {
                    SetIconCoolDown(characterName);
                    skill2IconCooldownMask.enabled = true;
                }
                else
                {
                    for (int i = 0; i < skill2CooldownTexts.Count; i++)
                    {
                        if (i != 1)
                        {
                            skill2CooldownTexts[i].enabled = false;
                        }
                    }
                    skill2IconCooldownMask.enabled = false;
                }
                if (cooldownController.moUSkillCurrentCoolDown>0)
                {

                }
                break;
            case "Lia":
                if (cooldownController.liaSkill1CurrentCoolDown > 0)
                {
                    SetIconCoolDown(characterName);
                    skill1IconCooldownMask.enabled = true;
                }
                else
                {
                    for (int i = 0; i < skill1CooldownTexts.Count; i++)
                    {
                        if (i != 2)
                        {
                            skill1CooldownTexts[i].enabled = false;
                        }
                    }
                    skill1IconCooldownMask.enabled = false;
                }
                if (cooldownController.liaSkill2CurrentCoolDown > 0)
                {
                    SetIconCoolDown(characterName);
                    skill2IconCooldownMask.enabled = true;
                }
                else
                {
                    for (int i = 0; i < skill2CooldownTexts.Count; i++)
                    {
                        if (i != 2)
                        {
                            skill2CooldownTexts[i].enabled = false;
                        }
                    }
                    skill2IconCooldownMask.enabled = false;
                }
                if (cooldownController.moUSkillCurrentCoolDown > 0)
                {

                }
                break;
        }
        SetButtonIconSprite(characterName);
    }
    /// <summary>
    /// ち传à︹ち传CD璸
    /// </summary>
    public void SetIconCoolDown(string characterName)
    {
        switch (characterName)
        {
            case "Mo":
                if (cooldownController.moSkill1CurrentCoolDown > 0)
                {
                    for (int i = 0; i < skill1CooldownTexts.Count; i++)
                    {
                        if (i == 1)
                            skill1CooldownTexts[i].enabled = true;
                        else
                            skill1CooldownTexts[i].enabled = false;
                    }
                }
                if (cooldownController.moSkill2CurrentCoolDowns[0] > 0 || cooldownController.moSkill2CurrentCoolDowns[1] > 0)
                {
                    for (int i = 0; i < skill2CooldownTexts.Count; i++)
                    {
                        if (i == 1)
                            skill2CooldownTexts[i].enabled = true;
                        else
                            skill2CooldownTexts[i].enabled = false;
                    }
                }
                if (cooldownController.moUSkillCurrentCoolDown > 0)
                {
                    for (int i = 0; i < USkillCooldownTexts.Count; i++)
                    {
                        if (i == 1)
                            USkillCooldownTexts[i].enabled = true;
                        else
                            USkillCooldownTexts[i].enabled = false;
                    }
                }
                break;
            case "Lia":
                if (cooldownController.liaSkill1CurrentCoolDown > 0)
                {
                    for (int i = 0; i < skill1CooldownTexts.Count; i++)
                    {
                        if (i == 2)
                            skill1CooldownTexts[i].enabled = true;
                        else
                            skill1CooldownTexts[i].enabled = false;
                    }
                }
                if (cooldownController.liaSkill2CurrentCoolDown > 0)
                {
                    for (int i = 0; i < skill2CooldownTexts.Count; i++)
                    {
                        if (i == 2)
                            skill2CooldownTexts[i].enabled = true;
                        else
                            skill2CooldownTexts[i].enabled = false;
                    }
                }
                if (cooldownController.liaUSkillCurrentCoolDown > 0)
                {
                    for (int i = 0; i < USkillCooldownTexts.Count; i++)
                    {
                        if (i == 2)
                            USkillCooldownTexts[i].enabled = true;
                        else
                            USkillCooldownTexts[i].enabled = false;
                    }
                }
                break;
        }
    }
    /// <summary>
    /// ち传à︹ち传м瓜
    /// </summary>
    public void SetButtonIconSprite(string characterName)
    {
        normalAttackIconSlot.sprite = normalAttackImages[characterName];
        skill1IconSlot.sprite = skill1SlotImages[characterName];
        skill2IconSlot.sprite = skill2SlotImages[characterName];
        USkillIconSlot.sprite = USkillSlotImages[characterName];

        if (skill2List[characterName].skillUseCount > 0)
        {
            skill2CountIconParent.SetActive(true);
        }
        else
        {
            skill2CountIconParent.SetActive(false);
        }
    }
    /// <summary>
    /// Lia盡ノち传妮┦ち传м籔ю阑icon
    /// </summary>
    public void LiaSetSkillIcon(ElementType element)
    {
        normalAttackImages["Lia"] = LiaNormalAttackSlotElementImages[element];
        skill1SlotImages["Lia"] = LiaSkill1SlotElementImages[element];
        skill2SlotImages["Lia"] = LiaSkill2SlotElementImages[element];
        skill1IconSlot.sprite = skill1SlotImages["Lia"];
        skill2IconSlot.sprite = skill2SlotImages["Lia"];
    }


}
