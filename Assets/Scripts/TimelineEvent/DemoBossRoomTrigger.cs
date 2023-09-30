using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DemoBossRoomTrigger : MonoBehaviour
{
    public PlayableDirector playableBehaviour;

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
