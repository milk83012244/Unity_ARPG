using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;


public class EnemyUnitType1 : Enemy
{
    private Rigidbody2D rig2D;
    private OtherCharacterStats stats;
    private ElementStatusEffect elementStatus;
    private Collider2D selfcollider2D;
    private BehaviorTree unitType1behaviorTree;

    public EnemyCurrentState currentState;

    public bool isAttackState;
    //���h�ĪG
    private float knockbackDuration = 0.1f;
    private bool isKnockbackActive;

    [Header("�{�{�ĪG��")]
    public float flashDuration = 0.2f; // �{�{����ɶ�
    public float flashInterval = 0.05f;
    public Color flashColor = Color.white; // �{�{�C��
    public SpriteRenderer spriteRenderer;
    public Color originalColor;
    private bool flashDurationCountEnd;

    //�w���ĪG
    public bool canStun;

    //�ݩ�2���ĪG
    public bool canIceStatus2;
    public bool iceStatus2Active;
    public float iceStatusDuration;

    private Coroutine IceStatus2Cor;

    public override void OnEnable()
    {
        currentState = EnemyCurrentState.Idle;
        isAttackState = false;

        stats.stunValueIsMaxAction += StartStunned;
        stats.elementStates2Action += StartElementStatus2;
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

    /// <summary>
    /// ���h�ĪG
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
    /// �ഫ��w�����A�íp��ɶ�
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
    /// <summary>
    /// �ݩ�2���ĪG
    /// </summary>
    public void StartElementStatus2(ElementType elementType)
    {
        switch (elementType)
        {
            case ElementType.Fire:
                break;
            case ElementType.Ice:
                if (canIceStatus2 && IceStatus2Cor ==null)
                    IceStatus2Cor = StartCoroutine(IceElementStatus2());
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
    private IEnumerator IceElementStatus2()
    {
        canStun = false;
        iceStatus2Active = true;
        currentState = EnemyCurrentState.Stop;
        canIceStatus2 = false;
        yield return Yielders.GetWaitForSeconds(iceStatusDuration);
        iceStatus2Active = false;
        elementStatus.SetElementActive(ElementType.Ice, false);
        currentState = EnemyCurrentState.Idle;
        canStun = true;
        StartCoroutine(IceStatus2CoolDown());
    }
    private IEnumerator IceStatus2CoolDown()
    {
        float cooldown = stats.enemyElementCountData.elementCoolDown[ElementType.Ice];
        cooldown -= iceStatusDuration;

        yield return Yielders.GetWaitForSeconds(cooldown);
        canIceStatus2 = true;
        if (IceStatus2Cor != null)
            IceStatus2Cor = null;
    }
    public void StopIceStatus2()
    {
        StopCoroutine(IceStatus2Cor);
        if (IceStatus2Cor != null)
            IceStatus2Cor = null;
    }
    public void StartFlash()
    {
        if (flashDurationCountEnd == false)
        {
            StartCoroutine(FlashDurationCount());
            StartCoroutine(FlashCoroutine());
        }
    }
    /// <summary>
    /// �Q�����ɰ{�{
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
    }
    private void DropItem()
    {

    }
    private void InitFlag()
    {
        canStun = true;
        canIceStatus2 = true;
        isAttackState = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);
    }

    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        unitType1behaviorTree.enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
}
