using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoUSkillEffect : MonoBehaviour
{
    private PlayerSkillManager skillManager;

    [HideInInspector] public SkillDataSO skillData;
    public PlayerCharacterStats characterStats;
    [HideInInspector] public PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
    }
    private void Start()
    {
        if (skillManager != null)
        {
            skillData = skillManager.skills[2];
        }
    }
}
