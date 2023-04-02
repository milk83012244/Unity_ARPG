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
        });

        return root;
    }

    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, fovRange);
    }
}
