using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ĤH���� �޲z�ۦP�ݩ�
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    protected Coroutine markClearCountCor; //�ɶ���۰ʮ����L�O

    public List<Transform> waypoints; //�����I
    public float moveSpeed ; //���ʳt��
    public float tempSpeed ; //�Ȧs�t��
    public float maxFovRange; //�̤j���Ľd��
    public float minFovRange ; //�̤p���Ľd��
    public float attackRange ; //�����d��

    [SerializeField] Transform TextPoolParent;
    [SerializeField] private GameObject damageTextPrafab; //�ˮ`��r
    private ObjectPool<DamageText> damageTextPool;

    [SerializeField] private SpriteRenderer[] markSprites; //�Q�ᤩ���L�O�Ϥ�

    public bool isMarked;
    private bool canMark = true;

    public virtual void OnEnable()
    {

    }
    public virtual void OnDisable()
    {

    }
    public virtual void Awake()
    {

    }
    public virtual void Start()
    {
        damageTextPool = ObjectPool<DamageText>.Instance;
        damageTextPool.InitPool(damageTextPrafab, 10, TextPoolParent);
    }
    public virtual void StopMove()
    {
        moveSpeed = 0;
    }
    public virtual void StartMove()
    {
        moveSpeed = tempSpeed;
    }
    /// <summary>
    /// �ͦ��ˮ`��r
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, bool isCritical = false, bool isSub = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, TextPoolParent);
        if (isSub)
            damageText.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            damageText.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        damageText.SetDamageText(takeDamage, isCritical);
    }
    /// <summary>
    /// �ͦ��ˮ`��r(�ݩʶˮ`)
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, ElementType elementType, bool isCritical = false,bool isSub=false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, TextPoolParent);
        if (isSub)
            damageText.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            damageText.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    /// <summary>
    /// �ͦ��ˮ`��r(�аO�ˮ`)
    /// </summary>
    public virtual void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f) + random, TextPoolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }

    /// <summary>
    /// �ᤩ�аO
    /// </summary>
    public virtual void SetMark(MarkType markType)
    {
        if (canMark)
        {
            markSprites[(int)markType].gameObject.SetActive(true);
            isMarked = true;
            markClearCountCor = StartCoroutine(MarkClearCount());
        }
    }
    /// <summary>
    /// �����аO
    /// </summary>
    public virtual void ClearMark()
    {
        StartCoroutine(CanMarkCoolDown());
        for (int i = 0; i < markSprites.Length; i++)
        {
            if (markSprites[i] != null)
            {
                Animator animator = markSprites[i].gameObject.GetComponent<Animator>();
                animator.SetTrigger("MarkClear");
            }
        }
        if (markClearCountCor != null)
        {
            StopCoroutine(markClearCountCor);
            markClearCountCor = null;
        }
        isMarked = false;
    }
    /// <summary>
    /// �аO�@�q�ɶ��S��Ĳ�o�N�ۤv�Ѱ�
    /// </summary>
    protected virtual IEnumerator MarkClearCount()
    {
        yield return Yielders.GetWaitForSeconds(5f);
        if (isMarked)
        {
            ClearMark();
        }
    }
    /// <summary>
    /// Ĳ�o�аO�᪺CD�ɶ�
    /// </summary>
    protected virtual IEnumerator CanMarkCoolDown()
    {
        canMark = false;
        yield return Yielders.GetWaitForSeconds(3f);
        canMark = true;
    }

    public virtual void DestroySelf()
    {
        Destroy(this.gameObject, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxFovRange);
    }

    public virtual void DamageEffect()
    {
        throw new System.NotImplementedException();
    }
}
