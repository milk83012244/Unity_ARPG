using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoAnimationEvent : MonoBehaviour
{
    public MoSkill1Attack skill1Attack;
    private int currentDirection;

    public void GetCurrentDirection()
    {
        currentDirection = skill1Attack.playerInput.currentDirection;
    }
    public void StartSpawnSkillEffect()
    {
        skill1Attack.SpawnSkillEffect(currentDirection);
    }
}
