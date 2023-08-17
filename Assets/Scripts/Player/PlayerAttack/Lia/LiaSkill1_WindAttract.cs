using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 吸引效果
/// </summary>
public class LiaSkill1_WindAttract : MonoBehaviour
{
    public float attractionForce = 1f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Rigidbody2D rb = collision.GetComponentInParent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 directionToBlackHole = transform.position - collision.transform.position;
                    float distance = directionToBlackHole.magnitude;
                    if (distance > 0.1f) // 避免極近的情況
                    {
                        Vector2 attraction = directionToBlackHole.normalized * (attractionForce / distance);
                        rb.AddForce(attraction);
                    }
                }
            }
        }
    }
}
