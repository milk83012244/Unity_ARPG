using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRotate : MonoBehaviour
{
    public Transform aimTransform;
    [SerializeField] private PlayerController playerController;

    public int currentDirection;

    void Update()
    {
        DriectionCheck();
        playerController.RotateAim(aimTransform);
    }
    /// <summary>
    /// 非みよ浪代
    /// </summary>
    private int DriectionCheck()
    {
        Vector3 aim = aimTransform.transform.position;
        Vector3 player = playerController.LiaProjectilePos.transform.position;
        Vector2 direction =  aim - player;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (-angle >= -45 && -angle <= 45) //(が传)
        {
            currentDirection = 3;
        }
        else if (-angle <= 135 && -angle > 45)//
        {
            currentDirection = 2;
        }
        else if (-angle > 135 && -angle <= 180 || -angle <= -135 && -angle >= -180)//オ(が传)
        {
            currentDirection = 1;
        }
        else if (-angle > -135 && -angle < -45)//
        {
            currentDirection = 4;
        }
        return currentDirection;
    }
}
