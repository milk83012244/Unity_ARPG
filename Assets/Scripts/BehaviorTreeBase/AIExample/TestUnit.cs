using System.Collections.Generic;
using Sx.BehaviorTree;

public class TestUnit : Tree
{
    public UnityEngine.Transform[] waypoints;
    public static float speed = 0.8f;
    public static float fovRange = 0.8f;
    public static float attackRange = 0.2f;
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
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, fovRange);
    }
}
