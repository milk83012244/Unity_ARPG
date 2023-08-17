using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Lia的屬性轉換
/// 負責:Lia攻擊屬性轉換,呼叫切換按鈕,按鈕功能禁用
/// </summary>
public class LiaElementSwitch : SerializedMonoBehaviour
{
    [SerializeField] private LiaElementSwitchButton liaElementSwitchButton;
    [SerializeField] private AttackButtons attackButtons;
    [SerializeField] private LiaUnlockDataSO liaUnlockData;

    private PlayerInput input;
    private PlayerCharacterStats characterStats;
    private LiaNormalAttack normalAttack;

    public static bool canSwitch;

    private void OnEnable()
    {
        SetDefaultElement();
    }
    private void Awake()
    {
        canSwitch = true;

        input = GetComponentInParent<PlayerInput>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        normalAttack = GetComponentInChildren<LiaNormalAttack>();
    }

    private void Update()
    {
        SwitchElementButton();
    }
    /// <summary>
    /// 切換到該角色時預設屬性
    /// </summary>
    private void SetDefaultElement()
    {
        SwitchElement(characterStats.attackData[2].elementType);
    }

    public void SwitchElement(ElementType elementType)
    {
        if (canSwitch == false)
        {
            Debug.Log(elementType + "無法切換屬性");
            return;
        }
        if (liaUnlockData.elementUnlockDic[elementType] == false)
        {
            Debug.Log( elementType + "屬性未解鎖");
            return;
        }

        input.canSwitchFunctionkey[(int)characterStats.attackData[2].elementType] = true;
        characterStats.attackData[2].elementType = elementType;
        characterStats.SetAttackElementType(elementType);
        input.canSwitchFunctionkey[(int)elementType] = false;

        //改變屬性UI
        liaElementSwitchButton.SetElementIcon(elementType);
        attackButtons.LiaSetSkillIcon(elementType);

        Debug.Log("切換到" + elementType);
    }
    /// <summary>
    /// 按鈕切換屬性
    /// </summary>
    private void SwitchElementButton()
    {
        if (input.SwitchFunctionkey1)
        {
            SwitchElement(ElementType.Fire);
        }
        else if (input.SwitchFunctionkey2)
        {
            SwitchElement(ElementType.Ice);
        }
        else if (input.SwitchFunctionkey3)
        {
            SwitchElement(ElementType.Thunder);
        }
        else if (input.SwitchFunctionkey4)
        {
            SwitchElement(ElementType.Wind);
        }
    }
}
