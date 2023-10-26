using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    public List<Transform> spawnPoints = new List<Transform>();
    public List<GameObject> WayPointsType = new List<GameObject>();

    public List<GameObject> enemyTypePrefabs = new List<GameObject>();

    public Transform TextPoolParent;
    public GameObject damageTextPrafab; //傷害文字
    public ObjectPool<DamageText> damageTextPool;

    public Transform EnemyProjectilepoolParent;
    public GameObject goblin_BprojectilePrefab; //遠距攻擊物件
    public ObjectPool<Goblin_B_Projectile> goblin_BprojectileEffectPool;

    private void Start()
    {
        SetDamageText();
        SetProjectilePool();
    }

    public void SpawnEnemyType1(GameObject enemyPrefab,int pointIndex)
    {
        GameObject enemysInstance = Instantiate(enemyPrefab, spawnPoints[pointIndex]);

        //設定巡邏點
        EnemyUnitType1 enemyUnitType1 = enemysInstance.GetComponent<EnemyUnitType1>();
        enemyUnitType1.SetEnemySpawner(this);
        SetWayPoint(enemyUnitType1, pointIndex);
    }
    public void SpawnEnemyType2(GameObject enemyPrefab, int pointIndex)
    {
        GameObject enemysInstance = Instantiate(enemyPrefab, spawnPoints[pointIndex]);
        enemies.Add(enemysInstance);

        //設定巡邏點
        EnemyUnitType2 enemyUnitType2 = enemysInstance.GetComponent<EnemyUnitType2>();
        enemyUnitType2.SetEnemySpawner(this);
        for (int i = 0; i < WayPointsType[pointIndex].transform.childCount; i++)
        {
            enemyUnitType2.waypoints.Add(WayPointsType[pointIndex].transform.GetChild(i));
        }
    }

    public DemoEnemyCounter demoEnemyCounter;
    public void SpawnEnemyDemo1()
    {
        SpawnEnemyType1(enemyTypePrefabs[0], 0);
        SpawnEnemyType1(enemyTypePrefabs[0], 1);
        SpawnEnemyType1(enemyTypePrefabs[0], 2);

        demoEnemyCounter.startCount = true;
    }
    public void SetWayPoint(EnemyUnitType1 enemyUnitType1,int pointIndex)
    {
        for (int i = 0; i < WayPointsType[pointIndex].transform.childCount; i++)
        {
            enemyUnitType1.waypoints.Add(WayPointsType[pointIndex].transform.GetChild(i));
        }
    }
    public void SpawnEnemyDebug1()
    {
        SpawnEnemyType1(enemyTypePrefabs[0], 0);
        //SpawnEnemyType2(enemyTypePrefabs[0], 0);
    }
    public void SpawnEnemyDebug2()
    {
        SpawnEnemyType2(enemyTypePrefabs[1], 1);
    }

    public void SetDamageText()
    {
        damageTextPool = ObjectPool<DamageText>.Instance;
        damageTextPool.InitPool(damageTextPrafab, 10, TextPoolParent);
    }
    public void SetProjectilePool()
    {
        goblin_BprojectileEffectPool = ObjectPool<Goblin_B_Projectile>.Instance; //子彈初始化
        goblin_BprojectileEffectPool.InitPool(goblin_BprojectilePrefab, 30, EnemyProjectilepoolParent);
    }
}
