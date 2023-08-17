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

        //��F�ؼ��I
        if (agent.reachedEndOfPath && !agent.pathPending && float.IsPositiveInfinity(switchTime))
        {
            reachPath = true;
            switchTime = Time.time + delay;
        }
        //���ݮɶ����� �����ؼ��I
        if (Time.time >= switchTime)
        {
            index = index + 1;
            search = true;
            switchTime = float.PositiveInfinity;
            reachPath = false;
        }

        //�p�G�w��C��� �N���s�}�l
        index = index % targets.Length;
        //��o�U�@�ӥؼ��I
        agent.destination = targets[index].position;

        //�p���F�ؼ��I�����|
        if (search) 
            agent.SearchPath();
    }
}
