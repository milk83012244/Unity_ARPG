using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.Events;
using Sirenix.Utilities;
using DG.Tweening;

/// <summary>
/// ���b����panel�W
/// </summary>
public class PlayerPartyManager : SerializedMonoBehaviour
{
    private CharacterSwitchButtons characterSwitchButtons;
    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isEnableShowEnd = false;

    public PartyDataSO partyData;
    public Dictionary<int, string> party = new Dictionary<int, string>();

    //�����ø
    public Dictionary<string, Sprite> characterBackgroundSlotImages = new Dictionary<string, Sprite>();
    //��������
    public Dictionary<string, Sprite> characterUnitSlotSlotImages = new Dictionary<string, Sprite>();

    public List<Transform> partySlot = new List<Transform>();
    public Dictionary<string, Toggle> characterToggles = new Dictionary<string, Toggle>();

    private void OnEnable()
    {
        InitPartyMenu();

        isEnableShowEnd = false;
        if (!isEnableShowEnd)
        {
            StartCoroutine(ShowPanelEffect());
        }
    }
    private void OnDisable()
    {
        ExitPartyMenu();
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        
    }
    private void Start()
    {
        //�]�w�C��toggle���}���ƥ�
        foreach (var kvp in characterToggles)
        {
            kvp.Value.onValueChanged.AddListener((isOn) => OnToggleValueExecute(kvp.Key, isOn));
        }
    }
    /// <summary>
    /// �K�[�����
    /// </summary>
    public void AddPartyMember(int partyId,string name)
    {
        if (party.Count < 3)
        {
            party.Add(partyId, name);
            //��sUI������
        }
    }
    /// <summary>
    /// ���������
    /// </summary>
    public void RemovePartyMember(int partyId)
    {
        if (party.ContainsKey(partyId))
        {
            party.Remove(partyId);
        }
        else
        {
            Debug.Log("�L������");
        }
    }
    public void OnToggleValueExecute(string characterName, bool isOn)
    {
        if (isOn)
        {
            for (int i = 1; i < party.Count+1; i++)
            {
                if (party[i].IsNullOrWhitespace())
                {
                    SetPartySlot(i, characterName);
                    party[i] = characterName;
                    break;
                }
            }
        }
        else
        {
            for (int i = 1; i < party.Count + 1; i++)
            {
                if (party[i] == characterName)
                {
                    RemoveSlot(i);
                    party[i] = "";
                    break;
                }
            }
        }
        RefreshSlot();
    }

    /// <summary>
    /// �i�J����O��l��
    /// </summary>
    public void InitPartyMenu()
    {
        characterSwitchButtons = FindObjectOfType<CharacterSwitchButtons>();
        //Ū��������
        if (partyData.currentParty.Values.Count > 0)
        {
            party = partyData.currentParty;
            for (int i = 1; i < party.Count + 1; i++)
            {
                if (!party[i].IsNullOrWhitespace())
                {
                    SetPartySlot(i, party[i]);
                    characterToggles[party[i]].isOn = true;
                }
            }
        }
        else
        {
            for (int i = 1; i < partySlot.Count; i++)
            {
                RemoveSlot(i);
            }
            foreach (KeyValuePair<string, Toggle> item in characterToggles)
            {
                characterToggles[item.Key].isOn = false;
            }
        }
    }
    /// <summary>
    /// �]�w����O���
    /// </summary>
    public void SetPartySlot(int slotNumber,string characterName)
    {
        Image characterUnitImage = partySlot[slotNumber].Find("CharacterUnitImage").GetComponent<Image>();
        Image characterBackgroundImage = partySlot[slotNumber].Find("CharacterBackgroundImage").GetComponent<Image>();
        TextMeshProUGUI nameText = partySlot[slotNumber].Find("StatusText").Find("Nametxt").GetComponent<TextMeshProUGUI>();

        characterUnitImage.sprite = characterUnitSlotSlotImages[characterName];
        characterBackgroundImage.sprite = characterBackgroundSlotImages[characterName];
        nameText.text = characterName;
    }
    public void RemoveSlot(int slotNumber)
    {
        Image characterUnitImage = partySlot[slotNumber].Find("CharacterUnitImage").GetComponent<Image>();
        Image characterBackgroundImage = partySlot[slotNumber].Find("CharacterBackgroundImage").GetComponent<Image>();
        TextMeshProUGUI nameText = partySlot[slotNumber].Find("StatusText").Find("Nametxt").GetComponent<TextMeshProUGUI>();
        characterUnitImage.sprite = null;
        characterBackgroundImage.sprite = null;
        nameText.text = "";
    }
    /// <summary>
    /// ����ܰʮɨ�s�Ƨ�
    /// </summary>
    public void RefreshSlot()
    {
        if (party.Values.Count > 0)
        {
            ////�M�Ŷ���C��
            //for (int i = 1; i < partySlot.Count; i++)
            //{
            //    RemoveSlot(i);
            //}
            //���s�ƦC����r��
            for (int i = 1; i < party.Keys.Count + 1; i++)
            {
                if (i < 3)
                {
                    if (party[i].IsNullOrWhitespace() && !party[i + 1].IsNullOrWhitespace())
                    {
                        party[i] = party[i + 1];
                        party[i + 1] = "";
                        RemoveSlot(i + 1);
                        SetPartySlot(i, party[i]);
                    }
                }
            }
        }
    }
    /// <summary>
    /// �h�X����O �}�l�]�w��L���
    /// </summary>
    public void ExitPartyMenu()
    {
        canvasGroup.DOFade(0, 0f);

        partyData.currentParty = party;
        characterSwitchButtons.SetCharacterSlotIcon(partyData);

        //�h�X�Ȱ����A
        GameManager.Instance.SetState(GameState.Normal);
    }

    private IEnumerator ShowPanelEffect()
    {
        canvasGroup.DOFade(1, 0.5f);
        yield return Yielders.GetWaitForSeconds(0.5f);
        isEnableShowEnd = true;
    }
}
