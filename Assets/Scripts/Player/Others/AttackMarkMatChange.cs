using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMarkMatChange : MonoBehaviour
{
    public SpriteRenderer Mo_markRenderer;
    public Texture Mo_MarkIn;
    public Texture Mo_MarkClear;

    public void ChangeInShaderSprite()
    {
        Mo_markRenderer.material.SetTexture("_GlowTex", Mo_MarkIn);
    }
    public void ChangeClearShaderSprite()
    {
        Mo_markRenderer.material.SetTexture("_GlowTex", Mo_MarkClear);
    }
    public void CloseActive()
    {
        this.gameObject.SetActive(false);
    }
}
