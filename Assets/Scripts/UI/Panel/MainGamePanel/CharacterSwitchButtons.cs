using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

public class CharacterSwitchButtons : SerializedMonoBehaviour
{
    public PartyDataSO partyData;

    public List<Transform> characterSwitchSlots = new List<Transform>();
    public Dictionary<string, Sprite> characterSwitchSlotImages = new Dictionary<string, Sprite>();

    public float switchCooldownTime;

    public bool canSwitch;

    //�N�o�ɶ������ƥ�
    [HideInInspector] public Action characterSwitchSlot1CountCooldownTrigger;
    [HideInInspector] public Action characterSwitchSlot2CountCooldownTrigger;
    [HideInInspector] public Action characterSwitchSlot3CountCooldownTrigger;

    //���s�O�_�i�Ψƥ�
    //���A���γ]�w�ҥθT�Ψ�������ƥ�
    [HideInInspector] public Action<bool> characterSwitchSlotCanUseForStateAction;
    [HideInInspector] public Action<bool> characterSwitchSlot1CanUseAction;
    [HideInInspector] public Action<bool> characterSwitchSlot2CanUseAction;
    [HideInInspector] public Action<bool> characterSwitchSlot3CanUseAction;

    public List<TextMeshProUGUI> characterSwitchSlotCooldownTexts = new List<TextMeshProUGUI>();

    private string currentCharacterName;

    private int currentSlot; //��e�����

    private Coroutine characterSwitchCor;
    private bool isCooldown;

    private void OnEnable()
    {
        characterSwitchSlotCanUseForStateAction += SetCharacterSwitchButtonForStateActive;
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        characterSwitchSlotCanUseForStateAction -= SetCharacterSwitchButtonForStateActive;
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
    }
    private void Awake()
    {
        canSwitch = true;
        if (partyData.currentParty.Values.Count>0)
        {
            SetCharacterSlotIcon(partyData);
        }
    }
    public void StartSwitchCooldown(string characterName)
    {
        characterSwitchCor = StartCoroutine(SwitchCDCount(characterName));
    }
    /// <summary>
    /// ���A���γ]�w�ҥθT�Ψ���������s
    /// </summary>
    public void SetCharacterSwitchButtonForStateActive(bool active)
    {
        if (isCooldown)
        {
            return;
        }
        if (GameManager.Instance.CurrentGameState == GameState.Battle &&  currentCharacterName.IsNullOrWhitespace())
        {
            return;
        }

        for (int i = 1; i < partyData.currentParty.Count + 1; i++)
        {
            if (currentCharacterName == partyData.currentParty[i])
            {
                currentSlot = i;
            }
        }

        if (active == false) //�T��
        {
            canSwitch = active;
            for (int i = 1; i < characterSwitchSlots.Count; i++)
            {
                if (i != currentSlot)
                {
                    Image slotImage = characterSwitchSlots[i].Find("CharacterIcon").GetComponent<Image>();
                    Color initialColor = slotImage.color;
                    initialColor = new Color(0.35f, 0.35f, 0.35f, 0.7f);
                    //initialColor.a = 0.5f;
                    slotImage.color = initialColor;
                }
            }
        }
        else //�ҥ�
        {
            canSwitch = true;
            for (int i = 1; i < characterSwitchSlots.Count; i++)
            {
                if (i != currentSlot)
                {
                    Image slotImage = characterSwitchSlots[i].Find("CharacterIcon").GetComponent<Image>();
                    Color initialColor = slotImage.color;
                    initialColor = new Color(1f, 1f, 1f, 1f);
                    slotImage.color = initialColor;
                }
            }
        }
    }
    public void SetCurrnetUseCharacter(string characterName)
    {
        currentCharacterName = characterName;
    }

    /// <summary>
    /// �������CD�p��
    /// </summary>
    private IEnumerator SwitchCDCount(string characterName)
    {
        isCooldown = true;

        float currentTime = switchCooldownTime;

        for (int i = 1; i < partyData.currentParty.Count + 1; i++)
        {
            if (characterName == partyData.currentParty[i])
            {
                currentSlot = i;
            }
        }
        for (int i = 1; i < characterSwitchSlots.Count; i++)
        {
            if (i!=currentSlot)
            {
                Image slotImage = characterSwitchSlots[i].Find("CharacterIcon").GetComponent<Image>();
                //Color initialColor = slotImage.color;
                //initialColor = new Color(0.35f,0.35f,0.35f,0.7f);
                //initialColor.a = 0.5f;
                slotImage.color = new Color(0.35f, 0.35f, 0.35f, 0.7f);
            }
        }
        for (int i = 1; i < characterSwitchSlotCooldownTexts.Count; i++)
        {
            if (i != currentSlot)
                characterSwitchSlotCooldownTexts[i].enabled = true;
        }

        while (currentTime >= 0) //CD��r�p��
        {
            currentTime -= Time.deltaTime;

            for (int i = 1; i < characterSwitchSlotCooldownTexts.Count; i++)
            {
                if (i != currentSlot)
                    characterSwitchSlotCooldownTexts[i].text = currentTime.ToString("F1");
            }
            yield return null;
        }
        for (int i = 1; i < characterSwitchSlots.Count; i++)
        {
            if (i != currentSlot)
            {
                Image slotImage = characterSwitchSlots[i].Find("CharacterIcon").GetComponent<Image>();
                //Color initialColor = slotImage.color;
                //initialColor = new Color(1f, 1f, 1f, 1f);
                slotImage.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        for (int i = 1; i < characterSwitchSlotCooldownTexts.Count; i++)
        {
            if (i != currentSlot)
                characterSwitchSlotCooldownTexts[i].enabled = false;
        }

        isCooldown = false;
    }
    /// <summary>
    /// �q�����Ƥ�������icon
    /// </summary>
    public void SetCharacterSlotIcon(PartyDataSO partyData)
    {
        for (int i = 1; i < partyData.currentParty.Count + 1; i++)
        {
            if (!partyData.currentParty[i].IsNullOrWhitespace())
            {
                Image slotImage = characterSwitchSlots[i].Find("CharacterIcon").GetComponent<Image>();
                slotImage.sprite = characterSwitchSlotImages[partyData.currentParty[i]];
                characterSwitchSlots[i].gameObject.SetActive(true);
            }
            else
            {
                characterSwitchSlots[i].gameObject.SetActive(false);
            }
        }
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Normal)
        {
            SetCharacterSwitchButtonForStateActive(true);
        }
    }
}
