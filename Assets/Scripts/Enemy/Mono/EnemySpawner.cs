using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [HideInInspector] public List<GameObject> enemies = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> WayPointsType1 = new List<GameObject>();
    public List<GameObject> enemyType1Prefabs = new List<GameObject>();

    public void SpawnEnemyType1(GameObject enemyPrefab,int pointIndex)
    {
        GameObject enemysInstance = Instantiate(enemyPrefab, spawnPoints[pointIndex]);
        enemies.Add(enemysInstance);

        //³]©w¨µÅÞÂI
        EnemyUnitType1 enemyUnitType1 = enemysInstance.GetComponent<EnemyUnitType1>();
        for (int i = 0; i < WayPointsType1[pointIndex].transform.childCount; i++)
        {
            enemyUnitType1.waypoints.Add(WayPointsType1[pointIndex].transform.GetChild(i));
        }
       
    }

    public void SpawnEnemyDebug1()
    {
        SpawnEnemyType1(enemyType1Prefabs[0], 0);
    }
    public void SpawnEnemyDebug2()
    {
        SpawnEnemyType1(enemyType1Prefabs[0], 1);
    }
}
