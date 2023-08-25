using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 共通屬性2階效果介面方法
/// </summary>
public interface ICharacterElement2Effect
{
    /// <summary>
    /// 啟動2階屬性效果
    /// </summary>
    public void StartElementStatus2(ElementType elementType);

    /// <summary>
    /// 可再次進入2階的冷卻時間
    /// </summary>
    public IEnumerator Status2CoolDown(ElementType elementType);

    /// <summary>
    /// 火2階 燃燒效果實作
    /// </summary>
    public IEnumerator FireElementStatus2();
    /// <summary>
    /// 火2階造成傷害實作
    /// </summary>
    public IEnumerator FireElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// 冰2階 冰凍效果實作
    /// </summary>
    public IEnumerator IceElementStatus2();

    /// <summary>
    /// 風2階 風切效果實作
    /// </summary>
    public IEnumerator WindElementStatus2();
    /// <summary>
    /// 風2階造成傷害實作
    /// </summary>
    public IEnumerator WindElementStatus2Damage(PlayerCharacterStats playerGiver=null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// 雷2階 麻痺效果實作
    /// </summary>
    public IEnumerator ThunderElementStatus2();
    /// <summary>
    /// 雷2階造成傷害實作
    /// </summary>
    public IEnumerator ThunderElementStatus2Damage(PlayerCharacterStats playerGiver = null, OtherCharacterStats otherGiver = null);

    /// <summary>
    /// 清除持續時間的協程
    /// </summary>
    public void Status2ActiveCorReset(ElementType elementType);

    /// <summary>
    /// 啟動2階混合效果
    /// </summary>
    public void StartElementStatus2MixEffect(ElementType elementType);

    /// <summary>
    /// 2階混合觸發擴散效果
    /// </summary>
    public IEnumerator ElementStatus2MixDiffusionTrigger(ElementType elementType);

    /// <summary>
    /// 強制解除2階狀態
    /// </summary>
    public void StopStatus2(ElementType elementType);
}
