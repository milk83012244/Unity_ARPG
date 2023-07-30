using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCursor : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerSkillManager skillManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        skillManager = GetComponentInParent<PlayerSkillManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        playerController.MoveSkillCursor(this.transform);
    }
    public void ChangeSprite(Sprite skillSprite)
    {
        spriteRenderer.sprite = skillSprite;
    }
}
