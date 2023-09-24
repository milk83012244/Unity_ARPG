using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DemoEnemyCounter : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject demoTimeline2;
    public GameObject demoTutorial;
    public PlayableDirector playableDirector;
    public Transform demoParent;
    public TestTemp testTemp;

    public bool startCount;

    private void Update()
    {
        if (startCount)
        {
            if (enemySpawner.enemies.Count <= 0)
            {
                Debug.Log("±Ò°ÊCutScene");
                demoTimeline2.SetActive(true);
                demoTutorial.SetActive(false);
                playableDirector.playableGraph.GetRootPlayable(0).Play();
                startCount = false;
                //Destroy(this.gameObject);
            }
        }
    }
}
