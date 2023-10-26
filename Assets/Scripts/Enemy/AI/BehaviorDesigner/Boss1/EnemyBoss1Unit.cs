using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Pathfinding;

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
    public Collider2D selfcollider2D;
    private BehaviorTree unitBossbehaviorTree;
    private CharacterElementCounter characterElementCounter;
    private AIPath aIPath;
    public EnemyCurrentState currentState;
    public Boss1AttackBehavior currentAttackBehavior; /*{ get; private set; }*/

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
    public bool isPositiveRange; //�n�����A�Х�
    public bool isKeepRange;

    public bool isActive; //�i�ιC�����A����Ұʪ��A
    public bool isDown;//���˪��A

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
    /// ���z�L�ͦ��覡����ĤH�ͦ���(�]���n�ϥμĤH�@�Ϊ����)
    /// </summary>
    public  void SetEnemySpawner()
    {
        this.enemySpawner = FindObjectOfType<EnemySpawner>();
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
        isDown = true;
        stats.enabled = false;
        selfcollider2D.enabled = false;
        //StopAllCoroutines();

        yield return Yielders.GetWaitForSeconds(3f);
        //Boss���Ѩƥ�b�ʵe�ƥ�Ĳ�o
        //this.gameObject.SetActive(false);
        //yield return Yielders.GetWaitForSeconds(0.5f);
        //Destroy(this.gameObject);
        GameManager.Instance.SetState(GameState.GameOver); //�C������
    }
    public void DamageByPlayer()
    {

    }
    public void SetAttackBehavior(Boss1AttackBehavior boss1AttackBehavior)
    {
        currentAttackBehavior = boss1AttackBehavior;
    }
    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
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
