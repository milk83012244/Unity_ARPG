using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

/// <summary>
/// 敵人基類 管理相同屬性
/// </summary>
public class Enemy : SerializedMonoBehaviour, IDamageable
{
    protected Coroutine markClearCountCor; //時間到自動消除印記
    public EnemySpawner enemySpawner;

    public List<Transform> waypoints; //巡邏點
    public float moveSpeed; //移動速度
    public float tempSpeed; //暫存速度
    public float maxFovRange; //最大索敵範圍
    public float minFovRange; //最小索敵範圍
    public float attackRange; //攻擊範圍

    public int typeID;//敵人類型(同一個type有多種類型的情況使用 預設0)

  //生成傷害文字物件池已交給敵人生成器

  [SerializeField] private SpriteRenderer[] markSprites; //被賦予的印記圖片
    [SerializeField] private GameObject noticeIcon; //驚嘆號物件
    protected Coroutine noticeIconRiseUpCor; //驚嘆號物件協程容器
    public float noticeIconUpScale;

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
    /// 獲得生成器
    /// </summary>
    public virtual void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }
    /// <summary>
    /// 生成傷害文字
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, bool isCritical = false, bool isSub = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = enemySpawner.damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, enemySpawner.TextPoolParent);
        if (isSub)
            damageText.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            damageText.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        damageText.SetDamageText(takeDamage, isCritical);
    }
    /// <summary>
    /// 生成傷害文字(屬性傷害)
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, ElementType elementType, bool isCritical = false,bool isSub=false,bool isBig=false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = enemySpawner.damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, enemySpawner.TextPoolParent);
        if (isSub)
            damageText.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else if(isBig)
            damageText.gameObject.transform.localScale = new Vector3(2f, 2f, 2f);
        else
            damageText.gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    /// <summary>
    /// 生成傷害文字(標記傷害)
    /// </summary>
    public virtual void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        DamageText damageText = enemySpawner.damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f) + random, enemySpawner.TextPoolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }

    /// <summary>
    /// 賦予標記
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
    /// 消除標記
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
    /// 標記一段時間沒有觸發就自己解除
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
    /// 觸發標記後的CD時間
    /// </summary>
    protected virtual IEnumerator CanMarkCoolDown()
    {
        canMark = false;
        yield return Yielders.GetWaitForSeconds(3f);
        canMark = true;
    }
    public virtual void StartNoticeIconRiseUpCor()
    {
        if (noticeIconRiseUpCor == null)
            noticeIconRiseUpCor = StartCoroutine(NoticeIconRiseUp());
    }
    /// <summary>
    /// 上升驚嘆號
    /// </summary>
    protected virtual IEnumerator NoticeIconRiseUp()
    {
        noticeIcon.SetActive(true);
        noticeIcon.transform.DOLocalMoveY(noticeIconUpScale,0.4f);
        yield return Yielders.GetWaitForSeconds(0.4f);
        yield return Yielders.GetWaitForSeconds(0.7f);
        noticeIcon.SetActive(false);
        noticeIcon.transform.DOLocalMoveY(0, 0f);
        noticeIconRiseUpCor = null;
    }

    public virtual void DestroySelf()
    {
        Destroy(this.gameObject, 1f);
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = UnityEngine.Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, maxFovRange);
    //}

    public virtual void DamageEffect()
    {
        throw new System.NotImplementedException();
    }
}
