using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTemp : MonoBehaviour
{
    public PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.GetInstance().isBattleMode)
        {
            GameManager.GetInstance().isBattleMode = true;
            playerController.BattleModeStartSwitchCharacter();
        }
        else if (GameManager.GetInstance().isBattleMode)
        {
            GameManager.GetInstance().isBattleMode = false;
            playerController.SwitchMainCharacter();
        }
    }
}
