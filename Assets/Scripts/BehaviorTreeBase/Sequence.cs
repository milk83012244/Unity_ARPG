using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// 行為樹條件順序節點(會遍歷所有符合的子節點執行)
    /// </summary>
    public class Sequence :Node
    {
        public Sequence() : base() { }
        /// <summary>
        /// 獲得順序節點的子節點
        /// </summary>
        public Sequence(List<Node> children) : base(children) { }

        /// <summary>
        /// 子節點狀態檢測
        /// </summary>
        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESS;
                        return state;
                }
            }
            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
            return state;
        }
    }
}

