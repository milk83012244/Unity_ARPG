using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// �欰�𪺭��`�I
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
        /// �K�[���`�I
        /// </summary>
        protected abstract Node SetupTree();
    }
}

