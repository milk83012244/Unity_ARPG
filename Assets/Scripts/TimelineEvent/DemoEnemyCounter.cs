using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoEnemyCounter : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    public bool startCount;

    private void Update()
    {
        if (startCount)
        {
            if (enemySpawner.enemies.Count <= 0)
            {
                Debug.Log("±Ò°ÊCutScene");

                Destroy(this);
            }
        }
    }
}
