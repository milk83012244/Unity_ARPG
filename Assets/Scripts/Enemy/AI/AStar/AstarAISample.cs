using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// �򥻪��M�����󲾰ʽd��
/// </summary>
public class AstarAISample : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    public Path path;

    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public float repathRate = 0.5f; //������|�v
    private float lastRepath = float.NegativeInfinity;
    public bool reachedEndOfPath;

    private void Start()
    {
        seeker = GetComponent<Seeker>();

        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);

        //StartCoroutine(SetMovePath());
    }
    private void Update()
    {
        //�w����|�I ���s�������j
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        }

        if (path == null)
        {
            return;
        }
        reachedEndOfPath = false; //�ˬd�O�_����I
        float distanceToWaypoint;
        while (true)
        {
            //�ˬd���|�I���Z��
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                //�ˬd�O�_�٦����|�I�άO����|���I
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        //����I�ɥ��ƴ�t
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        //�p��P�ؼЪ��¦V
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        Vector3 velocity = dir * speed * speedFactor;

        transform.position += velocity * Time.deltaTime;
    }
    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        //�]�w������|�� �s���˴��h�I��
        p.Claim(this);
        if (!p.error)
        {
            //���񴡥���|��
            if (path != null) path.Release(this);
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            path.Release(this);
        }
    }
}
