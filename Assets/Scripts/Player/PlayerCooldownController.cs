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

    private PlayerInput playerInput;

    private Coroutine dodgeCor;

    private void OnEnable()
    {
        DodgeCooldownTrigger += DodgeCooldownStart;
        Skill1CooldownTrigger += Skill1CooldownStart;
    }
    private void OnDisable()
    {
        DodgeCooldownTrigger -= DodgeCooldownStart;
        Skill1CooldownTrigger -= Skill1CooldownStart;
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
                break;
        }
    }
}
