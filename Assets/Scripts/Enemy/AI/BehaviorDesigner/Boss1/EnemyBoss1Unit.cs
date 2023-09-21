using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class EnemyBoss1Unit : Enemy
{
    /// <summary>
    /// �����欰���O
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

    //���h�ĪG
    private float knockbackDuration = 0f;
    private bool isKnockbackActive;

    //�w���ĪG
    public bool canStun;
    //����ˮ`�Х�
    public bool isDamaging;
    public float windDamageInterval;

    public float keepRange; //�i�J�O�u���A�d��

    //�ݩ�2���ĪG�@�Τ��Х�
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //�ݩ�2���ĪG����ɶ�
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //�ݩ�2���ĪG�@�Τ���{�e��
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
    #region ���h�P�w��
    /// <summary>
    /// ���h�ĪG �u���b�w���ɯ����h
    /// </summary>
    public void StartKnockback(Vector2 direction, float knockbackValue)
    {
        if (currentState == EnemyCurrentState.Stunning)
        {
            StartCoroutine(ApplyKnockback(direction, knockbackValue));
        }

    }
    /// <summary>
    /// ���h��V
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
    /// �ഫ��w�����A�íp�����ɶ�
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

    #region �ݩ�2������
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
