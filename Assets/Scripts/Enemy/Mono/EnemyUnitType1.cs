using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// 通用敵人1 負責接收狀態與觸發效果
/// </summary>
public class EnemyUnitType1 : Enemy
{
    private Rigidbody2D rig2D;
    private OtherCharacterStats stats;
    private ElementStatusEffect elementStatusEffect;
    private Collider2D selfcollider2D;
    private BehaviorTree unitType1behaviorTree;
    private CharacterElementCounter characterElementCounter;

    public EnemyCurrentState currentState;

    public bool isAttackState;
    //擊退效果
    private float knockbackDuration = 0.1f;
    private bool isKnockbackActive;

    [Header("閃爍效果區")]
    public float flashDuration = 0.2f; // 閃爍持續時間
    public float flashInterval = 0.05f;
    public Color flashColor = Color.white; // 閃爍顏色
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    private bool flashDurationCountEnd;

    //硬直效果
    public bool canStun;

    //持續傷害標示
    public bool isDamaging;

    public float windDamageInterval;

    //屬性2階效果作用中標示
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //屬性2階效果持續時間
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //屬性2階效果作用中協程容器
    [SerializeField] private Dictionary<ElementType, Coroutine> status2ActiveCor = new Dictionary<ElementType, Coroutine>();
    //2階擴散效果範圍
    public float searchRadius;
    public LayerMask enemiesLayer;

    public override void OnEnable()
    {
        currentState = EnemyCurrentState.Idle;
        isAttackState = false;

        stats.stunValueIsMaxAction += StartStunned;
        stats.enemyElementCountData.State2EffectTriggerEvent.AddListener(StartElementStatus2);
        stats.enemyElementCountData.State2MixEffectTriggerEvent.AddListener(StartElementStatus2MixEffect);

        stats.hpZeroEvent.AddListener(StartDeadCor);

        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    public override void OnDisable()
    {
        StopAllCoroutines();
        stats.enemyElementCountData.State2EffectTriggerEvent.RemoveListener(StartElementStatus2);
        stats.enemyElementCountData.State2MixEffectTriggerEvent.RemoveListener(StartElementStatus2MixEffect);

        GameManager.Instance.onNormalGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged -= OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged -= OnGameStateChanged;
    }

    public override void Awake()
    {
        InitFlag();

        currentState = EnemyCurrentState.Idle;
        rig2D = GetComponent<Rigidbody2D>();
        selfcollider2D = GetComponent<CircleCollider2D>();
        stats = GetComponent<OtherCharacterStats>();
        elementStatusEffect = GetComponentInChildren<ElementStatusEffect>();
        characterElementCounter = GetComponent<CharacterElementCounter>();
        unitType1behaviorTree = GetComponent<BehaviorTree>();
    }
    public override void Start()
    {
        for (int i = 0; i < status2ActiveDic.Keys.Count; i++)
        {
            status2ActiveDic[(ElementType)i] = false;
        }
        for (int i = 0; i < status2ActiveCor.Keys.Count; i++)
        {
            status2ActiveCor[(ElementType)i] = null;
        }
    }
    /// <summary>
    /// 被玩家造成傷害觸發相關狀態
    /// </summary>
    public void DamageByPlayer()
    {
        isAttackState = true;
        StartNoticeIconRiseUpCor();
    }
    #region 擊退與硬直
    /// <summary>
    /// 擊退效果
    /// </summary>
    public void StartKnockback(Vector2 direction, float knockbackValue)
    {
        StartCoroutine(ApplyKnockback(direction, knockbackValue));
    }
    /// <summary>
    /// 擊退方向
    /// </summary>
    private IEnumerator ApplyKnockback(Vector2 direction, float knockbackValue)
    {
        isKnockbackActive = true;
        rig2D.velocity = direction * (knockbackValue - stats.enemyBattleData.knockbackResistance);

        yield return Yielders.GetWaitForSeconds(knockbackDuration);

        rig2D.velocity = Vector2.zero;
        isKnockbackActive = false;
    }

    public void StartStunned(float stunTime)
    {
        StartCoroutine(Stunned(stunTime));
    }
    /// <summary>
    /// 轉換到硬直狀態並計算持續時間
    /// </summary>
    private IEnumerator Stunned(float stunTime)
    {
        currentState = EnemyCurrentState.Stunning;
        yield return Yielders.GetWaitForSeconds(stunTime);
        currentState = EnemyCurrentState.Idle;
        StartCoroutine(StunCoolDown());
    }
    private IEnumerator StunCoolDown()
    {
        canStun = false;
        yield return Yielders.GetWaitForSeconds(stats.enemyBattleData.stunCooldownTime);
        canStun = true;
    }
    #endregion

    #region 屬性2階效果相關
    /// <summary>
    /// 執行屬性2階效果
    /// </summary>
    public void StartElementStatus2(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                if (status2ActiveCor[elementType] == null)
                    status2ActiveCor[elementType] = StartCoroutine(IceElementStatus2());
                break;
            case ElementType.Wind:
                if (status2ActiveCor[elementType] == null)
                    status2ActiveCor[elementType] = StartCoroutine(WindElementStatus2());
                break;
            case ElementType.Thunder:
                break;
            case ElementType.Light:
                break;
            case ElementType.Dark:
                break;
        }
    }
    /// <summary>
    /// 冰2階 冰凍效果
    /// </summary>
    private IEnumerator IceElementStatus2()
    {
        Debug.Log("執行" + stats.baseCharacterData.characterName +  "的冰凍效果");
        canStun = false;
        status2ActiveDic[ElementType.Ice] = true;
        currentState = EnemyCurrentState.Stop;
        elementStatusEffect.SetElementActive(ElementType.Ice, true);

        yield return Yielders.GetWaitForSeconds(status2Duration[ElementType.Ice]);
        status2ActiveDic[ElementType.Ice] = false;
        elementStatusEffect.SetElementActive(ElementType.Ice, false);
        currentState = EnemyCurrentState.Idle;
        canStun = true;
        Status2ActiveCorReset(ElementType.Ice);
    }
    /// <summary>
    /// 風2階 風切效果
    /// </summary>
    private IEnumerator WindElementStatus2()
    {
        if (characterElementCounter.giverPlayerCharacterStats == null)
        {
            Debug.LogError("沒有收到給予屬性者的資料");
            yield break;
        }
        Debug.Log("執行" + stats.baseCharacterData.characterName + "的風切效果");

        PlayerCharacterStats giver = characterElementCounter.giverPlayerCharacterStats;
        isDamaging = true;

        status2ActiveDic[ElementType.Wind] = true;
        elementStatusEffect.SetElementActive(ElementType.Wind, true);
        StartCoroutine(WindElementStatus2Damage(giver));
        yield return Yielders.GetWaitForSeconds(status2Duration[ElementType.Wind]);

        isDamaging = false;
        status2ActiveDic[ElementType.Wind] = false;
        elementStatusEffect.SetElementActive(ElementType.Wind, false);
        Status2ActiveCorReset(ElementType.Wind);
    }
    /// <summary>
    /// 風2階造成傷害
    /// </summary>
    private IEnumerator WindElementStatus2Damage(PlayerCharacterStats playerGiver)
    {
        while (isDamaging)
        {
            //造成傷害
            playerGiver.TakeDamage(playerGiver, stats, ElementType.Wind, playerGiver.isCritical, isStatus2: true);
            this.SpawnDamageText(playerGiver.currentDamage, ElementType.Wind, isSub: true);
            yield return Yielders.GetWaitForSeconds(windDamageInterval);
        }
    }
    /// <summary>
    /// 清除持續時間的協程
    /// </summary>
    private void Status2ActiveCorReset(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                if (status2ActiveCor[ElementType.Ice] != null)
                {
                    StopCoroutine(status2ActiveCor[ElementType.Ice]);
                    status2ActiveCor[ElementType.Ice] = null;
                }
                break;
            case ElementType.Wind:
                if (status2ActiveCor[ElementType.Wind] != null)
                {
                    StopCoroutine(status2ActiveCor[ElementType.Wind]);
                    status2ActiveCor[ElementType.Wind] = null;
                }
                break;
            case ElementType.Thunder:
                break;
            case ElementType.Light:
                break;
            case ElementType.Dark:
                break;
        }
    }
    /// <summary>
    /// 啟動2階混合效果
    /// </summary>
    public void StartElementStatus2MixEffect(ElementType elementType)
    {
        //清除4屬2階效果的物件
        elementStatusEffect.ElementEffectObjectDeactivate();
        //關閉作用中的協程
        for (int i = 0; i < status2ActiveCor.Keys.Count; i++)
        {
            if ((ElementType)i != ElementType.Light || (ElementType)i != ElementType.Dark)
            {
                if (status2ActiveCor[(ElementType)i] != null)
                {
                    StopCoroutine(status2ActiveCor[(ElementType)i]);
                    status2ActiveCor[(ElementType)i] = null;
                }
            }
        }

        //觸發擴散效果
        StartCoroutine(ElementStatus2MixDiffusionTrigger(elementType));
    }
    /// <summary>
    /// 2階混合觸發擴散效果
    /// </summary>
    private IEnumerator ElementStatus2MixDiffusionTrigger(ElementType elementType)
    {
        PlayerCharacterStats giver = characterElementCounter.giverPlayerCharacterStats;

        yield return Yielders.GetWaitForSeconds(0.1f);
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemiesLayer);
        List<OtherCharacterStats> enemyList = new List<OtherCharacterStats>();
        foreach (Collider2D target in targets)
        {
            OtherCharacterStats otherCharacter = target.GetComponent<OtherCharacterStats>();
            //篩選賦予一階屬性的單位
            if (otherCharacter.enemyElementCountData.elementCountDic[elementType] == 1)
            {
                //賦予2階效果
                otherCharacter.characterElementCounter.AddElementCount(elementType, 2);

                //造成範圍傷害
                switch (elementType)
                {
                    case ElementType.Fire:
                        break;
                    case ElementType.Ice:
                        giver.TakeDamage(giver, otherCharacter, ElementType.Ice, giver.isCritical, isStatusMix: true,freeMul: 3f);
                        this.SpawnDamageText(giver.currentDamage, ElementType.Ice, isBig: true);
                        break;
                    case ElementType.Wind:
                        giver.TakeDamage(giver, otherCharacter, ElementType.Wind, giver.isCritical, isStatusMix: true, freeMul: 3f);
                        this.SpawnDamageText(giver.currentDamage, ElementType.Wind,isBig:true);
                        break;
                    case ElementType.Thunder:
                        break;
                    case ElementType.Light:
                        break;
                    case ElementType.Dark:
                        break;
                }

                enemyList.Add(otherCharacter);
            }
        }
        Debug.Log("共觸發" + enemyList.Count + "個目標");
    }

    public void StopStatus2(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                StopCoroutine(status2ActiveCor[ElementType.Ice]);
                if (status2ActiveCor[ElementType.Ice] != null)
                    status2ActiveCor[ElementType.Ice] = null;
                break;
            case ElementType.Wind:
                break;
            case ElementType.Thunder:
                break;
            case ElementType.Light:
                break;
            case ElementType.Dark:
                break;
        }
    }
    #endregion

    #region 閃爍效果
    public void StartFlash()
    {
        if (flashDurationCountEnd == false)
        {
            StartCoroutine(FlashDurationCount());
            StartCoroutine(FlashCoroutine());
        }
    }
    /// <summary>
    /// 被攻擊時閃爍
    /// </summary>
    private IEnumerator FlashCoroutine()
    {
        while (flashDurationCountEnd)
        {
            spriteRenderer.color = flashColor;
            yield return Yielders.GetWaitForSeconds(flashInterval);
            spriteRenderer.color = originalColor;
            yield return Yielders.GetWaitForSeconds(flashInterval);
        }
    }
    private IEnumerator FlashDurationCount()
    {
        flashDurationCountEnd = true;
        yield return Yielders.GetWaitForSeconds(flashDuration);
        flashDurationCountEnd = false;
    }
    #endregion

    public void StartDeadCor()
    {
        StartCoroutine(Dead());
    }
    private IEnumerator Dead()
    {
        currentState = EnemyCurrentState.Dead;
        stats.enabled = false;
        selfcollider2D.enabled = false;
        StopAllCoroutines();

        yield return Yielders.GetWaitForSeconds(1f);
        this.gameObject.SetActive(false);
        yield return Yielders.GetWaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
    private void DropItem()
    {

    }
    private void InitFlag()
    {
        canStun = true;
        isAttackState = false;
    }
    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        if (unitType1behaviorTree != null)
            unitType1behaviorTree.enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);

        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
