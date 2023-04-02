using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// �欰����󶶧Ǹ`�I(�|�M���Ҧ��ŦX���l�`�I����)
    /// </summary>
    public class Sequence :Node
    {
        public Sequence() : base() { }
        /// <summary>
        /// ��o���Ǹ`�I���l�`�I
        /// </summary>
        public Sequence(List<Node> children) : base(children) { }

        /// <summary>
        /// �l�`�I���A�˴�
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

