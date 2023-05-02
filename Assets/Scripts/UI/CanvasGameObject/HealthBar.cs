using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public CharacterStatsDataMutiMono characterMutiStats;
    public CharacterStatsDataMono characterStats;
    public TMP_Text hpText;

    void Update()
    {
        healthBar.fillAmount = GetExperienceNormalized();

        if (hpText != null)
        {
            if (characterStats != null && characterMutiStats==null)
            {
                hpText.text = characterStats.CurrnetHealth + "/" + characterStats.MaxHealth;
            }
            else if (characterStats == null && characterMutiStats != null)
            {
                hpText.text = characterMutiStats.CurrnetHealth + "/" + characterMutiStats.MaxHealth;
            }
        }

    }

    /// <summary>
    /// 獲得生命值的百分比
    /// </summary>
    public float GetExperienceNormalized()
    {
        if (characterStats != null && characterMutiStats == null)
        {
            return (float)characterStats.CurrnetHealth / characterStats.MaxHealth;
        }
        else if (characterStats == null && characterMutiStats != null)
        {
            return (float)characterMutiStats.CurrnetHealth / characterMutiStats.MaxHealth;
        }
        else
        {
            return 0;
        }
    }
}
