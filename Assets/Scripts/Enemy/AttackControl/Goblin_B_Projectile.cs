using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_B_Projectile : MonoBehaviour
{
    private OtherCharacterStats characterStats;
    private PlayerEffectSpawner playerEffectSpawner;

    public float speed;
    public float speedMultiplier;
    private float tempSpeed;
    public float projectileAngle = 90;
    private Vector3 direction;
    private Rigidbody2D rb2D;

    private void OnEnable()
    {
        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;

        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;
    }
    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        //collider2d = GetComponent<Collider2D>();
    }
    private void Start()
    {
        StartCoroutine(Recycle());
        tempSpeed = speed;
    }
    private void FixedUpdate()
    {
        rb2D.velocity = direction * speed * speedMultiplier;
    }
    /// <summary>
    /// 設定子彈方向與旋轉角度
    /// </summary>
    public void SetTargetPosition(Vector3 target, Vector3 self)
    {
        direction = (target - self).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 將旋轉角度應用到子彈
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public IEnumerator Recycle() //自己回收
    {
        yield return new WaitForSeconds(1f);
        ObjectPool<Goblin_B_Projectile>.Instance.Recycle(this);
    }
    public void NowRecycle()//立刻回收
    {
        ObjectPool<Goblin_B_Projectile>.Instance.Recycle(this);
    }
    /// <summary>
    /// 獲得角色數值
    /// </summary>
    public void GetCharacterStats(OtherCharacterStats characterStats)
    {
        this.characterStats = characterStats;
    }

    /// <summary>
    /// 造成傷害
    /// </summary>
    private void DealDamage(/*IDamageable damageable, */Collider2D collision)
    {
        PlayerCharacterStats playerCharacterStats = collision.GetComponent<PlayerCharacterStats>();
        PlayerEffectSpawner playerEffectSpawner = collision.GetComponent<PlayerEffectSpawner>();
        characterStats.isCritical = Random.value < characterStats.enemyAttackData.criticalChance; //爆擊判斷

        if (MoCounterCheck.guardCheckActive || MoCounterCheck.isGuardHit)
        {
            if (MoCounterCheck.guardCheckActive)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "觸發" + collision.name + "的反擊,傷害無效"));
            else if (MoCounterCheck.isGuardHit)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "反擊動作中,傷害無效"));
        }

        playerEffectSpawner.ballHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent); //生成擊中特效
        characterStats.TakeDamage(characterStats, playerCharacterStats, characterStats.isCritical);
        PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
        //賦予硬直值
        if (playerCharacterStats.GetPlayerCanStun())
            characterStats.TakeStunValue(characterStats, playerCharacterStats);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DealDamage(collision);
        NowRecycle();
    }
    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Paused || newGameState == GameState.GameOver)
        {
            speed = 0;
        }
        else
        {
            speed = tempSpeed;
        }
    }
}
