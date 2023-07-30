using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public PlayerCharacterStats PlayerCharacterStats;
    public OtherCharacterStats otherCharacterStats;
    public TMP_Text hpText;

    public Color fullHealthColor = Color.green;
    public Color middleHealthColor = Color.yellow;
    public Color lowHealthColor = Color.red;
    public float middleHealthThreshold = 0.45f; //血量顏色轉換值
    public float lowHealthThreshold = 0.25f;//血量顏色轉換值

    private void Start()
    {
        if (otherCharacterStats == null && PlayerCharacterStats != null)
        {
            healthBar.color = fullHealthColor;
            hpText.color = fullHealthColor;
        }
    }
    void Update()
    {
        healthBar.fillAmount = GetExperienceNormalized();

        if (hpText != null)
        {
            if (otherCharacterStats != null && PlayerCharacterStats == null)
            {
                hpText.text = otherCharacterStats.CurrnetHealth + "/" + otherCharacterStats.MaxHealth;
            }
            else if (otherCharacterStats == null && PlayerCharacterStats != null)
            {
                hpText.text = PlayerCharacterStats.CurrnetHealth + "/" + PlayerCharacterStats.MaxHealth;

                //血量顏色
                if (GetExperienceNormalized() <= middleHealthThreshold)
                {
                    healthBar.color = middleHealthColor;
                    hpText.color = middleHealthColor;
                }
                else if (GetExperienceNormalized() <= lowHealthThreshold)
                {
                    healthBar.color = lowHealthColor;
                    hpText.color = lowHealthColor;
                }
                else
                {
                    healthBar.color = fullHealthColor;
                    hpText.color = fullHealthColor;
                }
            }
        }

    }

    /// <summary>
    /// 獲得生命值的百分比
    /// </summary>
    public float GetExperienceNormalized()
    {
        if (otherCharacterStats != null && PlayerCharacterStats == null)
        {
            return (float)otherCharacterStats.CurrnetHealth / otherCharacterStats.MaxHealth;
        }
        else if (otherCharacterStats == null && PlayerCharacterStats != null)
        {
            return (float)PlayerCharacterStats.CurrnetHealth / PlayerCharacterStats.MaxHealth;
        }
        else
        {
            return 0;
        }
    }
}
