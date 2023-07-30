using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaElementSwitch : MonoBehaviour
{
    private PlayerInput input;
    private PlayerCharacterStats characterStats;
    private LiaNormalAttack normalAttack;

    [HideInInspector]public ElementType currentElementType;

    private void Awake()
    {
        input = GetComponentInParent<PlayerInput>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        normalAttack = GetComponentInChildren<LiaNormalAttack>();
    }
    private void Update()
    {
        SwitchElementButton();
    }

    public void SwitchElement(ElementType elementType)
    {
        currentElementType = elementType;
        characterStats.SetAttackElementType(elementType);
        //改變屬性UI
        Debug.Log("切換到" + elementType);
    }
    private void SwitchElementButton()
    {
        if (input.SwitchFunctionkey1)
        {
            SwitchElement(ElementType.Ice);
        }
        else if (input.SwitchFunctionkey2)
        {
            SwitchElement(ElementType.Fire);
        }
        else if (input.SwitchFunctionkey3)
        {
            SwitchElement(ElementType.Wind);
        }
        else if (input.SwitchFunctionkey4)
        {
            SwitchElement(ElementType.Thunder);
        }
    }
}
