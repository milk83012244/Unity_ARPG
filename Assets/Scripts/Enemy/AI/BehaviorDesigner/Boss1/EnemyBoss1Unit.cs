using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Pathfinding;

public class EnemyBoss1Unit : Enemy
{
    /// <summary>
    /// 攻擊行為指令
    /// </summary>
    public enum Boss1AttackBehavior
    {
        NormalAttack1,
        NormalAttack2,
        NormalAttack3,
        RangeAttack,
        SkillAttack,
        USkillAttack,
        WalkL,
        WalkR,
        BackOff,
        Near,
    }

    private Rigidbody2D rig2D;
    private OtherCharacterStats stats;
    private ElementStatusEffect elementStatusEffect;
    public Collider2D selfcollider2D;
    private BehaviorTree unitBossbehaviorTree;
    private CharacterElementCounter characterElementCounter;
    private AIPath aIPath;
    public EnemyCurrentState currentState;
    public Boss1AttackBehavior currentAttackBehavior; /*{ get; private set; }*/

    public bool inAttackRange;

    //擊退效果
    private float knockbackDuration = 0f;
    private bool isKnockbackActive;

    //硬直效果
    public bool canStun;
    //持續傷害標示
    public bool isDamaging;
    public float windDamageInterval;

    public float keepRange; //進入保守狀態範圍
    public bool isPositiveRange; //積極狀態標示
    public bool isKeepRange;

    public bool isActive; //可用遊戲狀態控制啟動狀態
    public bool isDown;//擊倒狀態

    //屬性2階效果作用中標示
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //屬性2階效果持續時間
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //屬性2階效果作用中協程容器
    [SerializeField] private Dictionary<ElementType, Coroutine> status2ActiveCor = new Dictionary<ElementType, Coroutine>();

    public override void OnEnable()
    {
        currentState = EnemyCurrentState.Idle;
        inAttackRange = false;
        SetEnemySpawner();

        stats.stunValueIsMaxAction += StartStunned;

        GameManager.Instance.onNormalGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onBattleGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onPasueGameStateChanged += OnGameStateChanged;
        GameManager.Instance.onGameOverGameStateChanged += OnGameStateChanged;
    }
    public override void OnDisable()
    {
        StopAllCoroutines();
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
        //selfcollider2D = GetComponent<CircleCollider2D>();
        stats = GetComponent<OtherCharacterStats>();
        elementStatusEffect = GetComponentInChildren<ElementStatusEffect>();
        aIPath = GetComponent<AIPath>();
        //characterElementCounter = GetComponent<CharacterElementCounter>();
        unitBossbehaviorTree = GetComponent<BehaviorTree>();
    }
    private void InitFlag()
    {
        canStun = true;
    }
    /// <summary>
    /// 不透過生成方式獲取敵人生成器(因為要使用敵人共用物件池)
    /// </summary>
    public  void SetEnemySpawner()
    {
        this.enemySpawner = FindObjectOfType<EnemySpawner>();
    }
    #region 擊退與硬直
    /// <summary>
    /// 擊退效果 只有在硬直時能擊退
    /// </summary>
    public void StartKnockback(Vector2 direction, float knockbackValue)
    {
        if (currentState == EnemyCurrentState.Stunning)
        {
            StartCoroutine(ApplyKnockback(direction, knockbackValue));
        }
    }
    /// <summary>
    /// 擊退方向
    /// </summary>
    private IEnumerator ApplyKnockback(Vector2 direction, float knockbackValue)
    {
        isKnockbackActive = true;
        aIPath.canMove = false;
        rig2D.velocity = direction * (knockbackValue - stats.enemyBattleData.knockbackResistance);

        yield return Yielders.GetWaitForSeconds(knockbackDuration);

        rig2D.velocity = Vector2.zero;
        aIPath.canMove = true;
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

    #region 屬性2階相關
    #endregion

    public void StartDeadCor()
    {
        StartCoroutine(Dead());
    }
    private IEnumerator Dead()
    {
        currentState = EnemyCurrentState.Dead;
        isDown = true;
        stats.enabled = false;
        selfcollider2D.enabled = false;
        //StopAllCoroutines();

        yield return Yielders.GetWaitForSeconds(3f);
        //Boss擊敗事件在動畫事件觸發
        //this.gameObject.SetActive(false);
        //yield return Yielders.GetWaitForSeconds(0.5f);
        //Destroy(this.gameObject);
        GameManager.Instance.SetState(GameState.GameOver); //遊戲結束
    }
    public void DamageByPlayer()
    {

    }
    public void SetAttackBehavior(Boss1AttackBehavior boss1AttackBehavior)
    {
        currentAttackBehavior = boss1AttackBehavior;
    }
    /// <summary>
    /// 在特定遊戲狀態下啟用
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        if (!isActive)
        {
            return;
        }

        if (unitBossbehaviorTree != null)
        {
            if (newGameState == GameState.Normal || newGameState == GameState.Battle)
            {
                unitBossbehaviorTree.enabled = true;
                aIPath.canMove = true;
            }
            else if (newGameState == GameState.Paused || newGameState == GameState.GameOver)
            {
                unitBossbehaviorTree.enabled = false;
                aIPath.canMove = false;
            }
        }
        if (GameManager.Instance.CurrentGameState == GameState.GameOver)
        {
            currentState = EnemyCurrentState.Stop;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);
        Gizmos.color = UnityEngine.Color.white;
        Gizmos.DrawWireSphere(transform.position, minFovRange);

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(transform.position, keepRange);
    }
}
