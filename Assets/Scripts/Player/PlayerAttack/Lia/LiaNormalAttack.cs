using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiaNormalAttack : MonoBehaviour
{
    private PlayerController controller;
    private PlayerEffectSpawner playerEffectSpawner;
    private LiaElementSwitch elementSwitch;
    public AimRotate aimRotate;
    public Transform aimTransform;
    public LiaSkill2RotateEffect liaSkill2RotateEffect;
    [HideInInspector] public PlayerCharacterStats characterStats;

    public Animator animator;

    public Transform poolParent;
    public GameObject projectilePrefab;
    private ObjectPool<Lia_NormalProjectile> projectileEffectPool;

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
        characterStats = GetComponentInParent<PlayerCharacterStats>();
        playerEffectSpawner = GetComponentInParent<PlayerEffectSpawner>();
    }
    private void Start()
    {
        SetProjectilePool();
    }
    /// <summary>
    /// 子彈初始化
    /// </summary>
    public void SetProjectilePool()
    {
        projectileEffectPool = ObjectPool<Lia_NormalProjectile>.Instance; //子彈初始化
        projectileEffectPool.InitPool(projectilePrefab, 30, poolParent);
    }

    /// <summary>
    /// 發射子彈
    /// </summary>
    public void FireBullet()
    {
        //Quaternion rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        Lia_NormalProjectile projectile = projectileEffectPool.Spawn(controller.transform.position+new Vector3(0,0.25f), poolParent);
        //bullet.transform.localPosition = this.transform.position;
        projectile.SetTargetPosition(aimTransform.position, controller.LiaProjectilePos.transform.position);
        projectile.GetCharacterStats(characterStats,playerEffectSpawner);
        projectile.GetLiaSkill2RotateEffect(liaSkill2RotateEffect);
    }

}
