using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaSkill2RotateEffect : MonoBehaviour
{
    [HideInInspector]public PlayerCharacterStats characterStats;
    [HideInInspector] public string characterName;

    public ElementType element;

    public List<GameObject> elementTypes;

    public float damageDuration = 3f;

    //¬O§_¦³½á¤©ÄÝ©Ê
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

    private void OnEnable()
    {
        StartCoroutine(ActiveCount());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator ActiveCount()
    {
        yield return new WaitForSeconds(damageDuration);
        this.gameObject.SetActive(false);
    }
    public void GetCharacterStats(PlayerCharacterStats characterStats)
    {
        this.characterStats = characterStats;
        this.characterName = this.characterStats.characterData[this.characterStats.currentCharacterID].characterName;
        SetElementType();
    }
    private void SetElementComponent(ElementType element)
    {
        
    }
    private void SetElementType()
    {
        for (int i = 0; i < elementTypes.Count; i++)
        {
            if (elementTypes[i] != null && elementTypes[i].activeSelf)
            {
                elementTypes[i].SetActive(false);
            }
        }
        element = characterStats.attackData[characterStats.currentCharacterID].elementType;
        switch (element)
        {
            case ElementType.Fire:
                fireElement = true;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Ice:
                fireElement = false;
                iceElement = true;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Wind:
                fireElement = false;
                iceElement = false;
                windElement = true;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
            case ElementType.Thunder:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = true;
                lightElement = false;
                darkElement = false;
                elementTypes[(int)element].SetActive(true);
                break;
        }
    }
}
