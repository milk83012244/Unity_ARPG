using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// �欰������ܾ��`�I(���ŦX���l�`�I�N�������U����)
    /// </summary>
    public class Selector : Node
    {
        public Selector() : base() { }
        /// <summary>
        /// ��o���Ǹ`�I���l�`�I
        /// </summary>
        public Selector(List<Node> children) : base(children) { }

        /// <summary>
        /// �l�`�I���A�˴�
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
            state = NodeState.FAILURE; //���S���F�������^����
            return state;
        }
    }
}
