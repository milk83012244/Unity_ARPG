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
    public float middleHealthThreshold = 0.45f; //��q�C���ഫ��
    public float lowHealthThreshold = 0.25f;//��q�C���ഫ��

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

                //��q�C��
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
    /// ��o�ͩR�Ȫ��ʤ���
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
