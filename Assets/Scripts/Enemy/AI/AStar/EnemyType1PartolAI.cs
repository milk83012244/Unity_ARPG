using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyType1PartolAI : MonoBehaviour
{
    public Transform[] targets;

    public float delay = 0;
    [HideInInspector] public int index;

    [HideInInspector] public bool reachPath;

    float switchTime = float.PositiveInfinity;
    IAstarAI agent;

     private void Awake()
    {
        agent = GetComponent<IAstarAI>();
    }
    private void Update()
    {
        if (targets.Length == 0)
            return;

        bool search = false;

        //到達目標點
        if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
        {
            reachPath = true;
            switchTime = Time.time + delay;
        }
        //等待時間結束 切換目標點
        if (Time.time >= switchTime)
        {
            index = index + 1;
            search = true;
            switchTime = float.PositiveInfinity;
            reachPath = false;
        }

        //如果已到列表尾 就重新開始
        index = index % targets.Length;
        //獲得下一個目標點
        agent.destination = targets[index].position;

        //計算到達目標點的路徑
        if (search) 
            agent.SearchPath();
    }
}
