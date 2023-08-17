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

        //�O�_�w�쨵���I
        if (agent.reachedEndOfPath && !agent.pathPending)
        {
            index = index + 1;
            search = true;
        }
        //�p�G�w��C��� �N���s�}�l
        index = index % targets.Length;
        agent.destination = targets[index].position;

        //�p���F�ؼ��I�����|
        if (search) agent.SearchPath();
    }
}
