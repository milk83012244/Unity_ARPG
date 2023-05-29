using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnits : MonoBehaviour,IDamageable
{
    private Coroutine markClearCountCor;

    public Transform[] waypoints;
    public float speed = 0.8f;
    public float tempSpeed = 0.8f;
    public float fovRange = 0.8f;
    public float attackRange = 0.3f;

    [SerializeField] Transform poolParent;
    [SerializeField] private GameObject damageTextPrafab;
    private ObjectPool<DamageText> damageTextPool;

    [SerializeField] private SpriteRenderer[] markSprites;

    public bool isMarked;

    private bool canMark = true;

    private void Start()
    {
        damageTextPool = ObjectPool<DamageText>.GetInstance();
        damageTextPool.InitPool(damageTextPrafab, 10, poolParent);
    }

    public  void StopMove()
    {
        speed = 0;
    }
    public  void StartMove()
    {
        speed = tempSpeed;
    }
    public void DamageEffect()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 生成傷害文字
    /// </summary>
    public void SpawnDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }
    public void SpawnDamageText(int takeDamage, ElementType elementType, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    public void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }

    /// <summary>
    /// 賦予標記
    /// </summary>
    public void SetMark(MarkType markType)
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
    public void ClearMark()
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
    private IEnumerator MarkClearCount()
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
    private IEnumerator CanMarkCoolDown()
    {
        canMark = false;
        yield return Yielders.GetWaitForSeconds(3f);
        canMark = true;
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject, 1f);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fovRange);
    }
}
