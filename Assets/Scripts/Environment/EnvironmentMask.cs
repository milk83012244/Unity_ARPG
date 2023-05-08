using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����˹����󨤦�b��誺�B�n(���b���a���W)
/// </summary>
public class EnvironmentMask : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�i�H���w�I�����F��A���B�n�p��

        if (transform.position.y > collision.transform.position.y)
        {
            SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null) //�]�w��������z����
            {
                spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        SpriteRenderer spriteRenderer = collision.gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) //�]�w��������z����
        {
            spriteRenderer.color = new(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        }
    }
}
