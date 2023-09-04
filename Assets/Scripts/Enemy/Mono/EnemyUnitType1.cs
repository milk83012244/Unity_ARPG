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
    private ElementStatusEffect elementStatusEffect;
    private Collider2D selfcollider2D;
    private BehaviorTree unitType1behaviorTree;
    private CharacterElementCounter characterElementCounter;

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

    //����ˮ`�Х�
    public bool isDamaging;

    public float windDamageInterval;

    //�ݩ�2���ĪG�@�Τ��Х�
    public Dictionary<ElementType, bool> status2ActiveDic = new Dictionary<ElementType, bool>();
    //�ݩ�2���ĪG����ɶ�
    public Dictionary<ElementType, float> status2Duration = new Dictionary<ElementType, float>();
    //�ݩ�2���ĪG�@�Τ���{�e��
    [SerializeField] private Dictionary<ElementType, Coroutine> status2ActiveCor = new Dictionary<ElementType, Coroutine>();
    //2���X���ĪG�d��
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
    /// �Q���a�y���ˮ`Ĳ�o�������A
    /// </summary>
    public void DamageByPlayer()
    {
        isAttackState = true;
        StartNoticeIconRiseUpCor();
    }
    #region ���h�P�w��
    /// <summary>
    /// ���h�ĪG
    /// </summary>
    public void StartKnockback(Vector2 direction, float knockbackValue)
    {
        StartCoroutine(ApplyKnockback(direction, knockbackValue));
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

    #region �ݩ�2���ĪG����
    /// <summary>
    /// �����ݩ�2���ĪG
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
    /// �B2�� �B��ĪG
    /// </summary>
    private IEnumerator IceElementStatus2()
    {
        Debug.Log("����" + stats.baseCharacterData.characterName +  "���B��ĪG");
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
    /// ��2�� �����ĪG
    /// </summary>
    private IEnumerator WindElementStatus2()
    {
        if (characterElementCounter.giverPlayerCharacterStats == null)
        {
            Debug.LogError("�S�����쵹���ݩʪ̪����");
            yield break;
        }
        Debug.Log("����" + stats.baseCharacterData.characterName + "�������ĪG");

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
    /// ��2���y���ˮ`
    /// </summary>
    private IEnumerator WindElementStatus2Damage(PlayerCharacterStats playerGiver)
    {
        while (isDamaging)
        {
            //�y���ˮ`
            playerGiver.TakeDamage(playerGiver, stats, ElementType.Wind, playerGiver.isCritical, isStatus2: true);
            this.SpawnDamageText(playerGiver.currentDamage, ElementType.Wind, isSub: true);
            yield return Yielders.GetWaitForSeconds(windDamageInterval);
        }
    }
    /// <summary>
    /// �M������ɶ�����{
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
    /// �Ұ�2���V�X�ĪG
    /// </summary>
    public void StartElementStatus2MixEffect(ElementType elementType)
    {
        //�M��4��2���ĪG������
        elementStatusEffect.ElementEffectObjectDeactivate();
        //�����@�Τ�����{
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

        //Ĳ�o�X���ĪG
        StartCoroutine(ElementStatus2MixDiffusionTrigger(elementType));
    }
    /// <summary>
    /// 2���V�XĲ�o�X���ĪG
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
            //�z��ᤩ�@���ݩʪ����
            if (otherCharacter.enemyElementCountData.elementCountDic[elementType] == 1)
            {
                //�ᤩ2���ĪG
                otherCharacter.characterElementCounter.AddElementCount(elementType, 2);

                //�y���d��ˮ`
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
        Debug.Log("�@Ĳ�o" + enemyList.Count + "�ӥؼ�");
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

    #region �{�{�ĪG
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
    /// �b�S�w�C�����A�U�ҥ�
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
