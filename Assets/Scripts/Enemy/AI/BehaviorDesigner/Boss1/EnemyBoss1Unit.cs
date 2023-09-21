using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

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
    private Collider2D selfcollider2D;
    private BehaviorTree unitType2behaviorTree;
    private CharacterElementCounter characterElementCounter;
    public EnemyCurrentState currentState;
    public Boss1AttackBehavior currentAttackBehavior;

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
    }
    public override void OnDisable()
    {
        StopAllCoroutines();
    }
    public override void Awake()
    {
        InitFlag();
        currentState = EnemyCurrentState.Idle;
        rig2D = GetComponent<Rigidbody2D>();
        selfcollider2D = GetComponent<CircleCollider2D>();
        stats = GetComponent<OtherCharacterStats>();
        elementStatusEffect = GetComponentInChildren<ElementStatusEffect>();
        //characterElementCounter = GetComponent<CharacterElementCounter>();
        unitType2behaviorTree = GetComponent<BehaviorTree>();
    }
    private void InitFlag()
    {
        canStun = true;
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

    #region 屬性2階相關
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
        //StopAllCoroutines();

        yield return Yielders.GetWaitForSeconds(1f);
        //this.gameObject.SetActive(false);
        yield return Yielders.GetWaitForSeconds(0.5f);
        //Destroy(this.gameObject);
    }
    public void SetAttackBehavior(Boss1AttackBehavior boss1AttackBehavior)
    {
        currentAttackBehavior = boss1AttackBehavior;
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
