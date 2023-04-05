using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public CharacterStatsDataMono characterStats;
    public TMP_Text hpText;

    void Update()
    {
        healthBar.fillAmount = GetExperienceNormalized();

        if (hpText != null)
        {
            hpText.text = characterStats.CurrnetHealth + "/" + characterStats.MaxHealth;
        }

    }

    /// <summary>
    /// ��o�ͩR�Ȫ��ʤ���
    /// </summary>
    public float GetExperienceNormalized()
    {
       return (float)characterStats.CurrnetHealth / characterStats.MaxHealth;
    }
}
