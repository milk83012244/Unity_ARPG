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
    private ElementStatusEffect elementStatus;
    private Collider2D selfcollider2D;
    private BehaviorTree unitType1behaviorTree;

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

    //屬性2階效果是否可觸發
    public Dictionary<ElementType, bool> canTriggerStatus2Dic = new Dictionary<ElementType, bool>();
    //屬性2階效果作用中標示
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //屬性2階效果持續時間
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //屬性2階效果作用中協程容器
    [SerializeField] private Dictionary<ElementType, Coroutine> status2ActiveCor = new Dictionary<ElementType, Coroutine>();

    public override void OnEnable()
    {
        currentState = EnemyCurrentState.Idle;
        isAttackState = false;

        stats.stunValueIsMaxAction += StartStunned;
        stats.elementStates2Action += StartElementStatus2;
        stats.elementStates2MixAction += StartElementStatus2MixEffect;

        stats.hpZeroEvent.AddListener(StartDeadCor);

        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    public override void OnDisable()
    {
        StopAllCoroutines();
        stats.stunValueIsMaxAction -= StartStunned;
        stats.elementStates2Action -= StartElementStatus2;

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
        elementStatus = GetComponentInChildren<ElementStatusEffect>();
        unitType1behaviorTree = GetComponent<BehaviorTree>();
    }
    public override void Start()
    {
        for (int i = 0; i < canTriggerStatus2Dic.Keys.Count; i++)
        {
            canTriggerStatus2Dic[(ElementType)i] = false;
        }
        for (int i = 0; i < status2ActiveDic.Keys.Count; i++)
        {
            status2ActiveDic[(ElementType)i] = false;
        }
    }

    /// <summary>
    /// 擊退效果
    /// </summary>
    public void StartKnockback(Vector2 direction,float knockbackValue)
    {
        StartCoroutine(ApplyKnockback(direction, knockbackValue));
    }
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
    /// 轉換到硬直狀態並計算時間
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
    #region 屬性2階效果相關
    /// <summary>
    /// 屬性2階效果
    /// </summary>
    public void StartElementStatus2(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                if (canTriggerStatus2Dic[ElementType.Ice] == true && status2ActiveCor[ElementType.Ice] == null)
                    status2ActiveCor[ElementType.Ice] = StartCoroutine(IceElementStatus2());
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
    /// <summary>
    /// 冰2階 冰凍效果
    /// </summary>
    private IEnumerator IceElementStatus2()
    {
        canStun = false;
        status2ActiveDic[ElementType.Ice] = true;
        currentState = EnemyCurrentState.Stop;
        canTriggerStatus2Dic[ElementType.Ice] = false;
        yield return Yielders.GetWaitForSeconds(status2Duration[ElementType.Ice]);
        status2ActiveDic[ElementType.Ice] = false;
        elementStatus.SetElementActive(ElementType.Ice, false);
        currentState = EnemyCurrentState.Idle;
        canStun = true;
        StartCoroutine(IceStatus2CoolDown(ElementType.Ice));
    }
    /// <summary>
    /// 風2階 風切效果
    /// </summary>
    /// <returns></returns>
    private IEnumerator WindElementStatus2()
    {
        canTriggerStatus2Dic[ElementType.Wind] = false;
        status2ActiveDic[ElementType.Wind] = true;
        //造成傷害

        yield return Yielders.GetWaitForSeconds(status2Duration[ElementType.Wind]);
        status2ActiveDic[ElementType.Wind] = false;
        elementStatus.SetElementActive(ElementType.Wind, false);
    }
    /// <summary>
    /// 可再次進入2階的冷卻時間
    /// </summary>
    private IEnumerator IceStatus2CoolDown(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                float cooldown = stats.enemyElementCountData.elementCoolDown[ElementType.Ice];
                cooldown -= status2Duration[ElementType.Ice];

                yield return Yielders.GetWaitForSeconds(cooldown);
                canTriggerStatus2Dic[ElementType.Ice] = true;
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

    public void StartElementStatus2MixEffect(ElementType elementType)
    {

    }

    public void StopStatus2(ElementType elementType) //強制解除2階狀態
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
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);
    }

    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        unitType1behaviorTree.enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
}
