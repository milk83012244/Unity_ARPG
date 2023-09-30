using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NiruAnimationEvent : MonoBehaviour
{
    private PlayerCharacterStats characterStats;

    private void Awake()
    {
        characterStats = GetComponentInParent<PlayerCharacterStats>();
    }

    public void StartDownEvent()
    {
        characterStats.SetInvincible(true);
    }
    public void EndDownEvent()
    {
        characterStats.SetInvincible(false);
    }
}
