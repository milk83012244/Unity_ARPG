using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sx.BehaviorTree
{
    /// <summary>
    /// 節點狀態
    /// </summary>
    public enum NodeState
    {
        RUNNING,//執行中
        SUCCESS,//完成
        FAILURE,//失敗
    }
    /// <summary>
    /// 行為樹節點基類
    /// </summary>
    public class Node
    {
        protected NodeState state;

        /// <summary>
        /// 父節點
        /// </summary>
        public Node parent;
        /// <summary>
        /// 子節點
        /// </summary>
        protected List<Node> children = new List<Node>();

        /// <summary>
        /// 節點資料儲存
        /// </summary>
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }
        /// <summary>
        /// 構造函數重載子節點
        /// </summary>
        public Node(List<Node> children)
        {
            foreach (var child in children)
            {
                _Attach(child);
            }
        }
        /// <summary>
        /// 連接節點
        /// </summary>
        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }
        /// <summary>
        /// 狀態檢測
        /// </summary>
        public virtual NodeState Evaluate() => NodeState.FAILURE;

        /// <summary>
        /// 設置目標資料
        /// </summary>
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        /// <summary>
        ///獲得任意資料
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
        /// 清除節點資料
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


