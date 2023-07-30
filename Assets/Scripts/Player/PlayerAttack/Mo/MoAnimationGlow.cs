using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 管理角色發光的Shader
/// </summary>
public class MoAnimationGlow : SerializedMonoBehaviour
{
    public SpriteRenderer Mo_SpriteRenderer;
    public Dictionary<string, Texture> MainTex = new Dictionary<string, Texture>();
    public Dictionary<string, Texture> GlowTex = new Dictionary<string, Texture>();

    public bool isUSkillState;

    private void OnEnable()
    {
        ResetColor();
    }
    public void SwitchMainTex(string AnimationState)
    {
        switch (AnimationState)
        {
            case "Idle":
                Mo_SpriteRenderer.material.SetTexture("_MainTex", MainTex["Idle"]);
                break;
            case "CounterHitL":
                Mo_SpriteRenderer.material.SetTexture("_MainTex", MainTex["CounterHitL"]);
                break;
            case "CounterHitR":
                Mo_SpriteRenderer.material.SetTexture("_MainTex", MainTex["CounterHitR"]);
                break;
        }
    }
    /// <summary>
    /// 切換角色動畫圖片遮罩
    /// </summary>
    public void SwitchGlowTex(string AnimationState)
    {
        if (isUSkillState)
            Mo_SpriteRenderer.material.SetTexture("_GlowTex", GlowTex[AnimationState]);
        else
            Mo_SpriteRenderer.material.SetTexture("_GlowTex", GlowTex[AnimationState]);
    }
    /// <summary>
    /// 切換發光顏色(沒有被遮罩擋住的地方會發光)
    /// </summary>
    public void SetGlowColor(string AnimationState)
    {
        switch (AnimationState)
        {
            case "CounterHitL":
                Mo_SpriteRenderer.material.SetColor("_GlowColor", new Color(0.85f, 0.79f, 0.3f,0f));
                break;
            case "CounterHitR":
                Mo_SpriteRenderer.material.SetColor("_GlowColor", new Color(0.85f, 0.79f, 0.3f, 0f));
                break;
        }
        //Mo_SpriteRenderer.material.SetColor("_GlowColor", new Color(0.85f, 0.79f, 0.3f));
    }
    public void ClearGlowTex()
    {
        Mo_SpriteRenderer.material.SetTexture("_GlowTex", null);
    }
    public void ResetColor()
    {
        Mo_SpriteRenderer.material.SetColor("_GlowColor", new Color(0f, 0f, 0f, 0f));
    }
}
