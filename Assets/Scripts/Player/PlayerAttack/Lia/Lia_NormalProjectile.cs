using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Lia的投射物控制
/// </summary>
public class Lia_NormalProjectile : MonoBehaviour
{
    private PlayerCharacterStats characterStats;
    private LiaSkill2RotateEffect liaSkill2RotateEffect;
    private PlayerEffectSpawner playerEffectSpawner;

    private Collider2D collider2d;

    public float speed;
    public float speedMultiplier;
    private float tempSpeed;
    public float projectileAngle = 90;
    //private Vector3 targetPosition;
    private Vector3 direction;
    private Rigidbody2D rb2D;

    public List<GameObject> projectileTypes;

    //是否有賦予屬性
    [HideInInspector] public bool fireElement;
    [HideInInspector] public bool iceElement;
    [HideInInspector] public bool windElement;
    [HideInInspector] public bool thunderElement;
    [HideInInspector] public bool lightElement;
    [HideInInspector] public bool darkElement;

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
        collider2d = GetComponent<Collider2D>();
    }
    private void Start()
    {
        StartCoroutine(Recycle());
        tempSpeed = speed;
    }
    //private void Update()
    //{
    //    // 將子彈往目標位置移動
    //    rb.velocity = direction * speed * Time.deltaTime;
    //}
    private void FixedUpdate()
    {
        rb2D.velocity = direction * speed * speedMultiplier;
    }
    /// <summary>
    /// 設定子彈方向與旋轉角度
    /// </summary>
    public void SetTargetPosition(Vector3 target,Vector3 player)
    {
        //targetPosition = target;
        // 計算子彈移動方向
        //direction = (transform.position -targetPosition).normalized;

        direction = (target - player).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // 將旋轉角度應用到子彈
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    /// <summary>
    /// 獲得角色數值
    /// </summary>
    public void GetCharacterStats(PlayerCharacterStats characterStats,PlayerEffectSpawner playerEffectSpawner)
    {
        this.characterStats = characterStats;
        this.playerEffectSpawner = playerEffectSpawner;
        SetElementType();
    }
    public void GetLiaSkill2RotateEffect(LiaSkill2RotateEffect liaSkill2RotateEffect)
    {
        this.liaSkill2RotateEffect = liaSkill2RotateEffect;
    }
    private void SetElementType()
    {
        for (int i = 0; i < projectileTypes.Count; i++)
        {
            if (projectileTypes[i]!= null && projectileTypes[i].activeSelf)
            {
                projectileTypes[i].SetActive(false);
            }
        }

        ElementType element = characterStats.attackData[characterStats.currentCharacterID].elementType;
        switch (element)
        {
            case ElementType.Fire:
                fireElement = true;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.Ice:
                fireElement = false;
                iceElement = true;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.Wind:
                fireElement = false;
                iceElement = false;
                windElement = true;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.Thunder:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = true;
                lightElement = false;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.Light:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = true;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.Dark:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = true;
                projectileTypes[(int)element].SetActive(true);
                break;
            case ElementType.None:
                fireElement = false;
                iceElement = false;
                windElement = false;
                thunderElement = false;
                lightElement = false;
                darkElement = false;
                projectileTypes[(int)element].SetActive(true);
                break;
        }
    }
    private void SetColliderSize()
    {

    }
    public IEnumerator Recycle() //自己回收
    {
        yield return new WaitForSeconds(1f);
        ObjectPool<Lia_NormalProjectile>.Instance.Recycle(this);
    }
    public void NowRecycle()//立刻回收
    {
        ObjectPool<Lia_NormalProjectile>.Instance.Recycle(this);
    }
    /// <summary>
    /// 造成傷害
    /// </summary>
    private void DealDamage(IDamageable damageable, Collider2D collision)
    {
        if (damageable == null)
        {
            return;
        }
        characterStats.isCritical = Random.value < characterStats.attackData[characterStats.currentCharacterID].criticalChance;
        if (damageable != null)
        {
            Enemy enemyUnit = damageable as Enemy;
            EnemyUnitType1 enemyUnitType1;
            EnemyUnitType2 enemyUnitType2;
            EnemyBoss1Unit enemyBoss1Unit;

            if (enemyUnit == null)
            {
                return;
            }

            OtherCharacterStats defander = enemyUnit.GetComponent<OtherCharacterStats>();

            #region 可傷害敵人共通
            if (fireElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Fire);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Fire);
            }
            else if (iceElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Ice, isCritical: characterStats.isCritical);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Ice, isCritical: characterStats.isCritical);
                defander.characterElementCounter.AddElementCount(ElementType.Ice, 1);
            }
            else if (windElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Wind);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Wind);
                defander.characterElementCounter.AddElementCount(ElementType.Wind, 1);
            }
            else if (thunderElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Thunder);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Thunder);
            }
            else if (lightElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Light);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Light);
            }
            else if (darkElement)
            {
                characterStats.TakeDamage(characterStats, defander, ElementType.Dark);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, ElementType.Dark);
            }
            if (enemyUnit.isMarked)
            {
                characterStats.TakeMarkDamage(characterStats, defander, characterStats.isCritical);
                enemyUnit.SpawnMarkDamageText(characterStats.currentDamage, characterStats.isCritical);

                enemyUnit.ClearMark();
            }

            //額外附加傷害
            //Lia技能2
            if (liaSkill2RotateEffect.gameObject.activeSelf)
            {
                characterStats.TakeSubDamage(characterStats, defander, liaSkill2RotateEffect.element, FromOthersName: liaSkill2RotateEffect.characterName);
                enemyUnit.SpawnDamageText(characterStats.currentDamage, liaSkill2RotateEffect.element, isSub: true);
            }

            playerEffectSpawner.ballHitEffectPool.Spawn(collision.transform.position, playerEffectSpawner.effectParent);

            #endregion
            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    //觸發敵人受擊狀態
                    enemyUnitType1.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType1.StartFlash();
                    //賦予敵人硬直值
                    if (enemyUnitType1.canStun)
                        characterStats.TakeStunValue(characterStats, defander);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    //觸發敵人受擊狀態
                    enemyUnitType2.DamageByPlayer();
                    //敵人閃爍效果
                    enemyUnitType2.StartFlash();
                    //賦予敵人硬直值
                    if (enemyUnitType2.canStun)
                        characterStats.TakeStunValue(characterStats, defander);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    //觸發敵人受擊狀態
                    enemyBoss1Unit.DamageByPlayer();
                    //敵人閃爍效果
                    //enemyBoss1Unit.StartFlash();
                    //賦予敵人硬直值
                    if (enemyBoss1Unit.canStun)
                        characterStats.TakeStunValue(characterStats, defander);
                    break;
            }
            Vector2 knockbackDirection;
            float knockbackValue;
            //造成敵人擊退
            switch (enemyUnit.typeID)
            {
                case 1:
                    enemyUnitType1 = enemyUnit as EnemyUnitType1;
                    knockbackDirection = (enemyUnitType1.transform.position - transform.position).normalized;
                     knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType1.StartKnockback(knockbackDirection, knockbackValue -= 0.5f);
                    break;
                case 2:
                    enemyUnitType2 = enemyUnit as EnemyUnitType2;
                    knockbackDirection = (enemyUnitType2.transform.position - transform.position).normalized;
                    knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
                    enemyUnitType2.StartKnockback(knockbackDirection, knockbackValue -= 0.5f);
                    break;
                case 1001:
                    enemyBoss1Unit = enemyUnit as EnemyBoss1Unit;
                    knockbackDirection = (enemyBoss1Unit.transform.position - transform.position).normalized;
                    knockbackValue = characterStats.attackData[characterStats.currentCharacterID].knockbackValue;
                    enemyBoss1Unit.StartKnockback(knockbackDirection, knockbackValue -= 0.5f);
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            DealDamage(damageable, collision);
            NowRecycle();
        }
    }

    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        if ( newGameState == GameState.Paused || newGameState == GameState.GameOver)
        {
            speed =0;
        }
        else
        {
            speed = tempSpeed;
        }
    }
}
