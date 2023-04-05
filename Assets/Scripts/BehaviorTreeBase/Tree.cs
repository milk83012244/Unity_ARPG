using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// 行為樹的首節點
    /// </summary>
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }
        private void Update()
        {
            if (_root != null)
                _root.Evaluate();
        }
        /// <summary>
        /// 添加首節點
        /// </summary>
        protected abstract Node SetupTree();
    }
}

