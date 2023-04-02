using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// 行為樹條件選擇器節點(找到符合的子節點就結束往下執行)
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        /// <summary>
        /// 獲得順序節點的子節點
        /// </summary>
        public Selector(List<Node> children) : base(children) { }

        /// <summary>
        /// 子節點狀態檢測
        /// </summary>
        public override NodeState Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESS:
                        state = NodeState.SUCCESS;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
            }
            state = NodeState.FAILURE; //都沒有達成條件返回失敗
            return state;
        }
    }
}
