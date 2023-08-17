using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

/// <summary>
/// �q�μĤH1 �t�d�������A�PĲ�o�ĪG
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

    //�ݩ�2���ĪG�O�_�iĲ�o
    public Dictionary<ElementType, bool> canTriggerStatus2Dic = new Dictionary<ElementType, bool>();
    //�ݩ�2���ĪG�@�Τ��Х�
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //�ݩ�2���ĪG����ɶ�
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //�ݩ�2���ĪG�@�Τ���{�e��
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
    #region �ݩ�2���ĪG����
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
    /// �B2�� �B��ĪG
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
    /// ��2�� �����ĪG
    /// </summary>
    /// <returns></returns>
    private IEnumerator WindElementStatus2()
    {
        canTriggerStatus2Dic[ElementType.Wind] = false;
        status2ActiveDic[ElementType.Wind] = true;
        //�y���ˮ`

        yield return Yielders.GetWaitForSeconds(status2Duration[ElementType.Wind]);
        status2ActiveDic[ElementType.Wind] = false;
        elementStatus.SetElementActive(ElementType.Wind, false);
    }
    /// <summary>
    /// �i�A���i�J2�����N�o�ɶ�
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

    public void StopStatus2(ElementType elementType) //�j��Ѱ�2�����A
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
    /// �b�S�w�C�����A�U�ҥ�
    /// </summary>
    private void OnGameStateChanged(GameState newGameState)
    {
        unitType1behaviorTree.enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    }
}
