using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TeleportPoint : MonoBehaviour
{
    public Transform destinationPos;//¥Ø¼ÐÂI
    //public Transform teleportPoint;
    public CinemachineConfiner2D mainCameraConfiner;
    public Collider2D cameraColider;
    public GameObject blackMask;

    public void Teleport(Transform playerPos)
    {
        StartDemoTeleportSceneChangeCor(playerPos);
    }
    public void StartDemoTeleportSceneChangeCor(Transform playerPos)
    {
        StartCoroutine(DemoTeleportSceneChange(playerPos));
    }
    private IEnumerator DemoTeleportSceneChange(Transform playerPos)
    {
        GameManager.Instance.SetState(GameState.Paused);
        blackMask.SetActive(true);
        Animator blackMaskAnimator = blackMask.GetComponent<Animator>();
        blackMaskAnimator.Play("BlackMaskFadeIn");
        blackMaskAnimator.speed = 2;
        yield return Yielders.GetWaitForSeconds(0.7f);
        mainCameraConfiner.m_BoundingShape2D = cameraColider;

        playerPos.position = destinationPos.position;
        yield return Yielders.GetWaitForSeconds(0.3f);
        blackMaskAnimator.Play("BlackMaskFadeOut");
        blackMaskAnimator.speed = 2;
        yield return Yielders.GetWaitForSeconds(1f);
        blackMask.SetActive(false);

        //GameManager.Instance.SetState(GameState.Normal);
    }
}
