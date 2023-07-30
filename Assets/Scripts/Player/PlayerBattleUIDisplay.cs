using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家遊戲中的戰鬥UI與特效顯示 從外部觸發
/// </summary>
public class PlayerBattleUIDisplay : MonoBehaviour
{
    [SerializeField] Transform poolParent; //相關UI物件池
    [SerializeField] private GameObject damageTextPrafab; //傷害文字
    private ObjectPool<DamageText> damageTextPool;

    public virtual void Start()
    {
        damageTextPool = ObjectPool<DamageText>.Instance;
        damageTextPool.InitPool(damageTextPrafab, 10, poolParent);
    }
    /// <summary>
    /// 生成傷害文字
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }
    /// <summary>
    /// 生成傷害文字(屬性傷害)
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, ElementType elementType, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    /// <summary>
    /// 生成傷害文字(標記傷害)
    /// </summary>
    public virtual void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }

}
