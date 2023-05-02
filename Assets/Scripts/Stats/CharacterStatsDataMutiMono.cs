using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ƨíp��(�i��������)
/// </summary>
public class CharacterStatsDataMutiMono : MonoBehaviour
{
    public int currentCharacterID;

    public List<CharacterBattleDataSO>  characterData = new List<CharacterBattleDataSO>();
    public List<CharacterAttackDataSO>  attackData = new List<CharacterAttackDataSO>();

    [HideInInspector] public int currentDamage;
    [HideInInspector] public bool isCritical;

    #region �qChatacterDataSOŪ����
    public int MaxHealth
    {
        get
        {
            if (characterData != null)
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
            if (characterData != null)
                return characterData[currentCharacterID].currentHealth;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentHealth = value;
        }
    }
    public int BaseDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].baseDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].baseDefence = value;
        }
    }
    public int CurrentDefence
    {
        get
        {
            if (characterData != null)
                return characterData[currentCharacterID].currentDefence;
            else
                return 0;
        }
        set
        {
            characterData[currentCharacterID].currentDefence = value;
        }
    }
    #endregion

    #region �ˮ`�p��
    /// <summary>
    /// �y���ˮ`(��o�Ĥ�}�����ͩR��)
    /// </summary>
    public void TakeDamage(CharacterStatsDataMutiMono attacker, CharacterStatsDataMono defender)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defender.CurrentDefence, 0); //Max�̤p�u�|�O0���|�ܭt��
        defender.CurrnetHealth = Mathf.Max(defender.CurrnetHealth - damage, 0);

        //UI��s
        //�g�紣�ɵ�
    }

    /// <summary>
    /// �p��ˮ`
    /// </summary>
    public int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData[currentCharacterID].minDamage, attackData[currentCharacterID].maxDamage);

        //�p����[�ˮ`

        if (isCritical)
        {
            coreDamage *= attackData[currentCharacterID].criticalMultplier;
            Debug.Log("�z��" + coreDamage);
        }
        currentDamage = (int)coreDamage;
        return (int)coreDamage;
    }
    #endregion
}
