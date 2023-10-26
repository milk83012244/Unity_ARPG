using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Pathfinding;

/// <summary>
/// �q�μĤH2 �t�d�������A�PĲ�o�ĪG
/// </summary>
public class EnemyUnitType2 : Enemy,ICharacterElement2Effect
{
    private Rigidbody2D rig2D;
    private OtherCharacterStats stats;
    private AIPath aIPath;
    private ElementStatusEffect elementStatusEffect;
    private Collider2D selfcollider2D;
    private BehaviorTree unitType2behaviorTree;
    private CharacterElementCounter characterElementCounter;

    public EnemyCurrentState currentState;

    public bool inAttackRange;
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
        aIPath = GetComponent<AIPath>();
    }
    private void InitFlag()
    {
        canStun = true;
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

    #region �ݩ�2������
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

    public IEnumerator Status2CoolDown(ElementType elementType)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator FireElementStatus2()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator FireElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator IceElementStatus2()
    {
        Debug.Log("����" + stats.baseCharacterData.characterName + "���B��ĪG");
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

    public IEnumerator WindElementStatus2()
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

    public IEnumerator WindElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator ThunderElementStatus2()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator ThunderElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null)
    {
        throw new System.NotImplementedException();
    }

    public void Status2ActiveCorReset(ElementType elementType)
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

    public void StartElementStatus2MixEffect(ElementType elementType)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator ElementStatus2MixDiffusionTrigger(ElementType elementType)
    {
        throw new System.NotImplementedException();
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


    public void EncounterPlayer()
    {
        StartNoticeIconRiseUpCor();
    }
    public void DamageByPlayer()
    {
 
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

    /// <summary>
    /// �b�S�w�C�����A�U�ҥ�
    /// </summary>
    //private void OnGameStateChanged(GameState newGameState)
    //{
    //    if (unitType1behaviorTree != null)
    //        unitType1behaviorTree.enabled = newGameState == GameState.Normal || newGameState == GameState.Battle;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);
        Gizmos.color = UnityEngine.Color.white;
        Gizmos.DrawWireSphere(transform.position, minFovRange);

        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
