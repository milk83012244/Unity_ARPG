using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaAnimationEvent : MonoBehaviour
{
    private PlayerCharacterStats characterStats;
    private PlayerEffectSpawner playerEffectSpawner;
    private PlayerCharacterSwitch characterSwitch;
    public LiaNormalAttack normalAttack;
    public LiaSkill1Spawner liaSkill1Spawner;
    public LiaSkill2Spawner liaSkill2Spawner;
    public LiaSkill2RotateEffect liaSkill2RotateEffect;
    public Collider2D playerCollider2D;

    public GameObject liaSkill2RotateEffectPrefab;

    public Transform skillCursorPostiton;

    private void Awake()
    {
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
        characterSwitch = GetComponentInParent<PlayerCharacterSwitch>();
    }

    public void FireBullet()
    {
        normalAttack.FireBullet();
    }
    public void StartDodgeMove()
    {
        playerCollider2D.enabled = false;
        characterStats.SetInvincible(true);
    }
    public void StartDownEvent()
    {
        characterStats.SetInvincible(true);
    }
    public void EndDodgeMove()
    {
        playerCollider2D.enabled = true;
        characterStats.SetInvincible(false);
        DodgeSmokeSpawn();
    }
    public void EndDownEvent()
    {
        characterStats.SetInvincible(false);
        characterSwitch.DownStateEnd();
    }
    public void DodgeSmokeSpawn()
    {
        playerEffectSpawner.DodgeSmokeTrigger.Invoke();
    }
    public void StartSpawnSkill1Effect()
    {
        liaSkill1Spawner.SpawnEffect(skillCursorPostiton);
    }
    public void StartSpawnSkill2Effect()
    {
        liaSkill2Spawner.SpawnEffect();
    }
    public void SpawnSkill2RotateEffect()
    {
        liaSkill2RotateEffect.GetCharacterStats(liaSkill2Spawner.characterStats);
        liaSkill2RotateEffectPrefab.SetActive(true);
    }


}
