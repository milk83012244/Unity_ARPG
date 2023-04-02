using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// �`�I���A
    /// </summary>
    public enum NodeState
    {
        RUNNING,//���椤
        SUCCESS,//����
        FAILURE,//����
    }
    /// <summary>
    /// �欰��`�I����
    /// </summary>
    public class Node
    {
        protected NodeState state;

        /// <summary>
        /// ���`�I
        /// </summary>
        public Node parent;
        /// <summary>
        /// �l�`�I
        /// </summary>
        protected List<Node> children = new List<Node>();

        /// <summary>
        /// �`�I����x�s
        /// </summary>
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }
        /// <summary>
        /// �c�y��ƭ����l�`�I
        /// </summary>
        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                _Attach(child);
            }
        }
        /// <summary>
        /// �s���`�I
        /// </summary>
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        /// <summary>
        /// ���A�˴�
        /// </summary>
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        /// <summary>
        /// �]�m�ؼи��
        /// </summary>
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        /// <summary>
        ///��o���N���
        /// </summary>
        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        /// <summary>
        /// �M���`�I���
        /// </summary>
        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}


