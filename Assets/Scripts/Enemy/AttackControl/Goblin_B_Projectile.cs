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
    /// �]�w�l�u��V�P���ਤ��
    /// </summary>
    public void SetTargetPosition(Vector3 target, Vector3 self)
    {
        direction = (target - self).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // �N���ਤ�����Ψ�l�u
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public IEnumerator Recycle() //�ۤv�^��
    {
        yield return new WaitForSeconds(1f);
        ObjectPool<Goblin_B_Projectile>.Instance.Recycle(this);
    }
    public void NowRecycle()//�ߨ�^��
    {
        ObjectPool<Goblin_B_Projectile>.Instance.Recycle(this);
    }
    /// <summary>
    /// ��o����ƭ�
    /// </summary>
    public void GetCharacterStats(OtherCharacterStats characterStats)
    {
        this.characterStats = characterStats;
    }

    /// <summary>
    /// �y���ˮ`
    /// </summary>
    private void DealDamage(/*IDamageable damageable, */Collider2D collision)
    {
        PlayerCharacterStats playerCharacterStats = collision.GetComponent<PlayerCharacterStats>();
        PlayerEffectSpawner playerEffectSpawner = collision.GetComponent<PlayerEffectSpawner>();
        characterStats.isCritical = Random.value < characterStats.enemyAttackData.criticalChance; //�z���P�_

        if (MoCounterCheck.guardCheckActive || MoCounterCheck.isGuardHit)
        {
            if (MoCounterCheck.guardCheckActive)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", "Ĳ�o" + collision.name + "������,�ˮ`�L��"));
            else if (MoCounterCheck.isGuardHit)
                Debug.Log(string.Format("<color=#ff0000>{0}</color>", collision.name + "�����ʧ@��,�ˮ`�L��"));
        }

        playerEffectSpawner.ballHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent); //�ͦ������S��
        characterStats.TakeDamage(characterStats, playerCharacterStats, characterStats.isCritical);
        PlayerController.GetInstance().uIDisplay.SpawnDamageText(characterStats.currentDamage, characterStats.isCritical);
        //�ᤩ�w����
        if (playerCharacterStats.GetPlayerCanStun())
            characterStats.TakeStunValue(characterStats, playerCharacterStats);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DealDamage(collision);
        NowRecycle();
    }
    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
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
