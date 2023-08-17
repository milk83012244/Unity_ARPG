using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PatrolExample : MonoBehaviour
{
    public Transform[] targets;

    int index;

    IAstarAI agent;

    void Awake()
    {
        agent = GetComponent<IAstarAI>();
    }
    private void Update()
    {
        if (targets.Length == 0) return;

        bool search = false;

        //是否已到巡邏點
        if (agent.reachedEndOfPath && !agent.pathPending)
        {
            index = index + 1;
            search = true;
        }
        //如果已到列表尾 就重新開始
        index = index % targets.Length;
        agent.destination = targets[index].position;

        //計算到達目標點的路徑
        if (search) agent.SearchPath();
    }
}
