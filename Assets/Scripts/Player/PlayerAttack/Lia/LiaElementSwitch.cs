using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// Lia���ݩ��ഫ
/// �t�d:Lia�����ݩ��ഫ,�I�s�������s,���s�\��T��
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
    /// ������Ө���ɹw�]�ݩ�
    /// </summary>
    private void SetDefaultElement()
    {
        SwitchElement(characterStats.attackData[2].elementType);
    }

    public void SwitchElement(ElementType elementType)
    {
        if (canSwitch == false)
        {
            Debug.Log(elementType + "�L�k�����ݩ�");
            return;
        }
        if (liaUnlockData.elementUnlockDic[elementType] == false)
        {
            Debug.Log( elementType + "�ݩʥ�����");
            return;
        }

        input.canSwitchFunctionkey[(int)characterStats.attackData[2].elementType] = true;
        characterStats.attackData[2].elementType = elementType;
        characterStats.SetAttackElementType(elementType);
        input.canSwitchFunctionkey[(int)elementType] = false;

        //�����ݩ�UI
        liaElementSwitchButton.SetElementIcon(elementType);
        attackButtons.LiaSetSkillIcon(elementType);

        Debug.Log("������" + elementType);
    }
    /// <summary>
    /// ���s�����ݩ�
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
