using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a�C�������԰�UI�P�S����� �q�~��Ĳ�o
/// </summary>
public class PlayerBattleUIDisplay : MonoBehaviour
{
    [SerializeField] Transform poolParent; //����UI�����
    [SerializeField] private GameObject damageTextPrafab; //�ˮ`��r
    private ObjectPool<DamageText> damageTextPool;

    public virtual void Start()
    {
        damageTextPool = ObjectPool<DamageText>.Instance;
        damageTextPool.InitPool(damageTextPrafab, 10, poolParent);
    }
    /// <summary>
    /// �ͦ��ˮ`��r
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }
    /// <summary>
    /// �ͦ��ˮ`��r(�ݩʶˮ`)
    /// </summary>
    public virtual void SpawnDamageText(int takeDamage, ElementType elementType, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.3f, 0.3f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f) + random, poolParent);
        damageText.SetDamageText(takeDamage, elementType, isCritical);
    }
    /// <summary>
    /// �ͦ��ˮ`��r(�аO�ˮ`)
    /// </summary>
    public virtual void SpawnMarkDamageText(int takeDamage, bool isCritical = false)
    {
        Vector3 random = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.2f) + random, poolParent);
        damageText.SetDamageText(takeDamage, isCritical);
    }

}
