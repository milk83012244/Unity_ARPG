using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �w�I�^����Ҧ�����HP
/// </summary>
public class HPRecovor : MonoBehaviour
{
    PlayerCharacterStats playerCharacterStats;
    PlayerCharacterSwitch playerCharacterSwitch;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerCharacterStats = collision.GetComponent<PlayerCharacterStats>();
        playerCharacterSwitch = collision.GetComponent<PlayerCharacterSwitch>();

        if (playerCharacterStats == null)
        {
            return;
        }
        if (playerCharacterStats != null)
        {
            //�D��HP�^�_
            playerCharacterStats.characterData[0].currentHealth = (int)playerCharacterStats.characterData[0].maxStunValue;

            if (playerCharacterSwitch.partyData.currentParty.Values.Count == 0)
            {
                return;
            }
            //�M������
            for (int i = 1; i < playerCharacterSwitch.partyData.currentParty.Keys.Count + 1; i++)
            {
                for (int j = 1; j < playerCharacterStats.characterData.Count; j++)
                {
                    if (playerCharacterStats.characterData[j].characterName == playerCharacterSwitch.partyData.currentParty[i])
                    {
                        //�ˬd�O�_���ˤU���A
                        if (playerCharacterStats.characterData[j].isDown)
                        {
                            //�_��
                            playerCharacterStats.characterData[j].currentHealth = (int)playerCharacterStats.characterData[j].maxStunValue;
                        }
                        else
                        {
                            playerCharacterStats.characterData[j].currentHealth = (int)playerCharacterStats.characterData[j].maxStunValue;
                        }
                    }
                }
            }

            //�^�_����⪺HP
        }
    }
}
