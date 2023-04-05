using System.Collections.Generic;
using Sx.BehaviorTree;
using UnityEngine;

public class TestUnit : Sx.BehaviorTree.Tree
{
    public UnityEngine.Transform[] waypoints;
    public static float speed = 0.8f;
    public static float fovRange = 0.8f;
    public static float attackRange = 0.3f;

    private static float tempSpeed = 0.8f;

    [SerializeField] Transform poolParent;
    [SerializeField] private GameObject damageTextPrafab;
    private ObjectPool<DamageText> damageTextPool;

    protected override void Start()
    {
        base.Start();
        damageTextPool = ObjectPool<DamageText>.GetInstance();
        damageTextPool.InitPool(damageTextPrafab, 10, poolParent);
    }

    protected override Node SetupTree()
    {
        // Node root = new TaskPatrol(transform, waypoints); //�K�[���ަ欰�`�I

        Node root = new Selector(new List<Node> //��ܾ�
        {
            new Sequence(new List<Node> //���ǰ���:�¥ؼЧ����d��
            {
                new TestCheckPlayerInAttackRange(transform),
                new TaskAttack(transform),
            }),
            new Sequence(new List<Node> //���ǰ���:�M��ؼнd��
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
    /// �ͦ��ˮ`��r
    /// </summary>
    public void SpawnDamageText(int takeDamage)
    {
        DamageText damageText = damageTextPool.Spawn(transform.position + new Vector3(0, 0.1f), poolParent);
        damageText.SetDamageText(takeDamage);
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject, 1f);
    }
    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, fovRange);
    }
}
