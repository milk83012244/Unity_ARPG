using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCooldownController : MonoBehaviour
{
    [HideInInspector]public Action DodgeCooldownTrigger;
    public float dodgeCooldownTime;

    [HideInInspector] public Action<string> Skill1CooldownTrigger;
    public MoSkill1Attack moSkill1Attack;

    [HideInInspector] public Action<string> Skill2CooldownTrigger;
    public MoSkill2Attack moSkill2Attack;

    private PlayerInput playerInput;

    public AttackButtons attackButtons;
    private Coroutine dodgeCor;

    private void OnEnable()
    {
        DodgeCooldownTrigger += DodgeCooldownStart;
        Skill1CooldownTrigger += Skill1CooldownStart;
        Skill2CooldownTrigger += Skill2CooldownStart;
    }
    private void OnDisable()
    {
        DodgeCooldownTrigger -= DodgeCooldownStart;
        Skill1CooldownTrigger -= Skill1CooldownStart;
        Skill2CooldownTrigger -= Skill2CooldownStart;
        StopAllCoroutines();
    }
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void DodgeCooldownStart()
    {
        dodgeCor = StartCoroutine(DodgeCooldown());
    }
    private IEnumerator DodgeCooldown()
    {
        playerInput.canDodge = false;
        yield return Yielders.GetWaitForSeconds(dodgeCooldownTime);
        playerInput.canDodge = true;
    }
    private void Skill1CooldownStart(string characterame)
    {
        switch (characterame)
        {
            case "Mo":
                moSkill1Attack.StartSkillCoolDown();
                attackButtons.StartSkill1Count(characterame);
                break;
        }
    }
    private void Skill2CooldownStart(string characterame)
    {
        switch (characterame)
        {
            case "Mo":
                moSkill2Attack.StartSkillCoolDown();
                attackButtons.StartSkill2Count(characterame);
                break;
        }
    }
}
