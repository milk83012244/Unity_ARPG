using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin_BAnimateEvent : MonoBehaviour
{
    public Goblin_BNormalAttack normalAttack;
    public void FireBullet()
    {
        normalAttack.FireProjectile();
    }
}
