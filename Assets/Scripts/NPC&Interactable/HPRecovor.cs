using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定點回隊伍所有角色HP
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
            //主角HP回復
            playerCharacterStats.characterData[0].currentHealth = (int)playerCharacterStats.characterData[0].maxStunValue;

            if (playerCharacterSwitch.partyData.currentParty.Values.Count == 0)
            {
                return;
            }
            //遍歷隊伍
            for (int i = 1; i < playerCharacterSwitch.partyData.currentParty.Keys.Count + 1; i++)
            {
                for (int j = 1; j < playerCharacterStats.characterData.Count; j++)
                {
                    if (playerCharacterStats.characterData[j].characterName == playerCharacterSwitch.partyData.currentParty[i])
                    {
                        //檢查是否為倒下狀態
                        if (playerCharacterStats.characterData[j].isDown)
                        {
                            //復活
                            playerCharacterStats.characterData[j].currentHealth = (int)playerCharacterStats.characterData[j].maxStunValue;
                        }
                        else
                        {
                            playerCharacterStats.characterData[j].currentHealth = (int)playerCharacterStats.characterData[j].maxStunValue;
                        }
                    }
                }
            }

            //回復隊伍角色的HP
        }
    }
}
