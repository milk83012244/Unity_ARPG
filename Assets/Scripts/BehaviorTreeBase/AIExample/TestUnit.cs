using System.Collections;
using System.Collections.Generic;
using Sx.BehaviorTree;
using UnityEngine;

public class TestUnit : Sx.BehaviorTree.Tree, IDamageable
{
    private Coroutine markClearCountCor;

    public UnityEngine.Transform[] waypoints;
    public static float speed = 0.8f;
    public static float fovRange = 0.8f;
    public static float attackRange = 0.3f;

    private static float tempSpeed = 0.8f;

    [SerializeField] Transform poolParent;
    [SerializeField] private GameObject damageTextPrafab;
    private ObjectPool<DamageText> damageTextPool;

    [SerializeField] private SpriteRenderer[] markSprites;

    public bool isMarked;

    protected override void Start()
    {
        base.Start();
        damageTextPool = ObjectPool<DamageText>.GetInstance();
        damageTextPool.InitPool(damageTextPrafab, 10, poolParent);
    }

    protected override Node SetupTree()
    {
        // Node root = new TaskPatrol(transform, waypoints); //添加巡邏行為節點

        Node root = new Selector(new List<Node> //選擇器
        {
            new Sequence(new List<Node> //順序執行:朝目標攻擊範圍
            {
                new TestCheckPlayerInAttackRange(transform),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node> //順序執行:尋找目標範圍
            {
                new TestCheckPlayerInFOVRange(transform),
                new TaskGoToTarget(transform),
            }),
            new TaskPatrol(transform,waypoints),
            new TaskDead(transform),
        });

        return root;
    }

    public static void StopMove()
    {
        ;
        speed = 0;
    }
    public static void StartMove()
    {
        speed = tempSpeed;
    }
    /// <summary>
    /// 生成傷害文字
    /// </summary>
    public void SpawnDamageText(int takeDamage, bool isCritical = false)
    {
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f), poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }
    public void SpawnDamageText(int takeDamage, ElementType elementType,bool isCritical =false)
    {
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f), poolParent);
        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    public void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f), poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject, 1f);
    }

    /// <summary>
    /// 賦予標記
    /// </summary>
    public void SetMark(MarkType markType)
    {
        markSprites[(int)markType].gameObject.SetActive(true);
        isMarked = true;
        markClearCountCor = StartCoroutine(MarkClearCount());
    }
    /// <summary>
    /// 消除標記
    /// </summary>
    public void ClearMark()
    {
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

    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, fovRange);
    }
    /// <summary>
    /// 實作受傷效果(硬直,擊退,特殊狀態等)
    /// </summary>
    public void DamageEffect()
    {
        
    }
}
