using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DemoBossRoomTrigger : MonoBehaviour
{
    public PlayableDirector playableBehaviour;
    public EnemyBoss1Unit boss1Unit;

    public void ActiveBoss1()
    {
        boss1Unit.isActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }
        playableBehaviour.Play();
        this.gameObject.SetActive(false);
    }
}
