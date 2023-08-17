using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/// <summary>
/// 基本的尋路插件移動範例
/// </summary>
public class AstarAISample : MonoBehaviour
{
    public Transform targetPosition;

    private Seeker seeker;
    public Path path;

    public float speed = 2;
    public float nextWaypointDistance = 3;
    private int currentWaypoint = 0;
    public float repathRate = 0.5f; //重算路徑率
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
        //已到路徑點 重新巡路間隔
        if (Time.time > lastRepath + repathRate && seeker.IsDone())
        {
            lastRepath = Time.time;
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        }

        if (path == null)
        {
            return;
        }
        reachedEndOfPath = false; //檢查是否到終點
        float distanceToWaypoint;
        while (true)
        {
            //檢查路徑點的距離
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                //檢查是否還有路徑點或是到路徑終點
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
        //到終點時平滑減速
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        //計算與目標的朝向
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;

        Vector3 velocity = dir * speed * speedFactor;

        transform.position += velocity * Time.deltaTime;
    }
    public void OnPathComplete(Path p)
    {
        Debug.Log("A path was calculated. Did it fail with an error? " + p.error);
        //設定插件路徑池 連續檢測多點用
        p.Claim(this);
        if (!p.error)
        {
            //釋放插件路徑池
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
