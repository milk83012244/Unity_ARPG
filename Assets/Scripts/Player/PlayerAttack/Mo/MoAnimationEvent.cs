using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MoAnimationEvent : SerializedMonoBehaviour
{
    public MoSkill1Attack skill1Attack;
    public MoCounterCheck counterCheck;
    public MoCounterAttack CounterAttack;
    public MoAnimationGlow animationGlow;
    public Collider2D playerCollider2D;
    private PlayerCharacterSwitch characterSwitch;
    private PlayerCharacterStats characterStats;
    private int currentDirection;

    private void Awake()
    {
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        characterSwitch = GetComponentInParent<PlayerCharacterSwitch>();
    }
    #region �ʵe�}�l�ƥ�
    public void StartIdleAnimateEvent()
    {
        animationGlow.ClearGlowTex();
        animationGlow.ResetColor();
    }
    public void StartBattleIdleAnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("BattleIdleL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("BattleIdleR");
                break;
        }
    }
    public void StartWalkAnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("WalkL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("WalkR");
                break;
            case "F":
                animationGlow.SwitchGlowTex("WalkF");
                break;
            case "B":
                animationGlow.SwitchGlowTex("WalkB");
                break;
        }
    }
    public void StartRunAnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("RunL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("RunR");
                break;
            case "F":
                animationGlow.SwitchGlowTex("RunF");
                break;
            case "B":
                animationGlow.SwitchGlowTex("RunB");
                break;
        }
    }
    public void StartDodgeAnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("DodgeL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("DodgeR");
                break;
        }
    }
    public void DodgeEvent2()
    {
        characterStats.SetInvincible(true);
        playerCollider2D.enabled = false;
    }
    public void StartAttack1AnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("Attack1L");
                break;
            case "R":
                animationGlow.SwitchGlowTex("Attack1R");
                break;
            case "F":
                animationGlow.SwitchGlowTex("Attack1F");
                break;
            case "B":
                animationGlow.SwitchGlowTex("Attack1B");
                break;
        }
    }
    public void StartAttack2AnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("Attack2L");
                break;
            case "R":
                animationGlow.SwitchGlowTex("Attack2R");
                break;
            case "F":
                animationGlow.SwitchGlowTex("Attack2F");
                break;
            case "B":
                animationGlow.SwitchGlowTex("Attack2B");
                break;
        }
    }
    public void StartAttack3AnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("Attack3L");
                break;
            case "R":
                animationGlow.SwitchGlowTex("Attack3R");
                break;
            case "F":
                animationGlow.SwitchGlowTex("Attack3F");
                break;
            case "B":
                animationGlow.SwitchGlowTex("Attack3B");
                break;
        }
    }
    public void StartSkill1AnimateEvent(string direction)
    {
        GetCurrentDirection();

        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("Skill1L");
                break;
            case "R":
                animationGlow.SwitchGlowTex("Skill1R");
                break;
            case "F":
                animationGlow.SwitchGlowTex("Skill1F");
                break;
            case "B":
                animationGlow.SwitchGlowTex("Skill1B");
                break;
        }
    }
    public void StartSkill2AnimateEvent(string direction)
    {
        playerCollider2D.enabled = false;
        characterStats.SetInvincible(true);

        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("Skill2L");
                break;
            case "R":
                animationGlow.SwitchGlowTex("Skill2R");
                break;
            case "F":
                animationGlow.SwitchGlowTex("Skill2F");
                break;
            case "B":
                animationGlow.SwitchGlowTex("Skill2B");
                break;
        }
    }
    public void StartGuardHitCheck(string direction)
    {
        StartCoroutine(counterCheck.GuardCheck());

        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("GuardHitL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("GuardHitR");
                break;
        }
    }
    public void StartGuardHitAnimateEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("CounterHitL");
                animationGlow.SetGlowColor("CounterHitL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("CounterHitR");
                animationGlow.SetGlowColor("CounterHitR");
                break;
        }

        MoCounterCheck.guardCheckActive = false;
    }
    public void StartCounterAttackEvent(string direction)
    {
        switch (direction)
        {
            case "L":
                animationGlow.SwitchGlowTex("CounterAttackL");
                break;
            case "R":
                animationGlow.SwitchGlowTex("CounterAttackR");
                break;
            case "F":
                animationGlow.SwitchGlowTex("CounterAttackF");
                break;
            case "B":
                animationGlow.SwitchGlowTex("CounterAttackB");
                break;
        }
    }
    public void StartDownEvent()
    {
        characterStats.SetInvincible(true);
    }
    #endregion

    #region �ʵe�����ƥ�
    public void EndSkill1AnimateEvent()
    {

    }
    public void EndSkill2AnimateEvent()
    {
        characterStats.SetInvincible(false);
        playerCollider2D.enabled = true;
    }
    public void EndDodgeEvent()
    {
        characterStats.SetInvincible(false);
        playerCollider2D.enabled = true;
    }
    public void EndCounterAttackEvent()
    {
        counterCheck.ResetState();
    }
    public void EndDownEvent()
    {
        characterStats.SetInvincible(false);
        characterSwitch.DownStateEnd();
    }
    #endregion

    #region ��L�ʵe�ƥ�
    #endregion


    public void GetCurrentDirection()
    {
        currentDirection = skill1Attack.playerInput.currentDirection;
    }
    public void StartSpawnSkillEffect()
    {
        skill1Attack.SpawnSkillEffect(currentDirection);
    }
}
