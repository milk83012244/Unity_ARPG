/// <summary>
/// 數值修改類型
/// </summary>
public enum StatModType
{
    Flat = 100,//整數
    PercentAdd = 200,//疊加百分比
    PercentMult = 300,//倍率增加百分比
}
/// <summary>
/// 數值修改器(例如角色能力添加 BUFF 裝備 物品等)
/// </summary>
public class StatModifier
{
    /// <summary>
    /// 修改器的值
    /// </summary>
    public readonly float Value;

    public readonly StatModType Type;

    /// <summary>
    /// 優先級
    /// </summary>
    public readonly int Order;

    /// <summary>
    /// 接收所有類型來源
    /// </summary>
    public readonly object Source;

    /// <summary>
    /// 初始化
    /// </summary>
    public StatModifier(float value, StatModType type,int order,object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, StatModType type) : this(value, type, (int)type ,null) { }

    public StatModifier(float value, StatModType type,int order) : this(value, type, order, null) { }

    public StatModifier(float value, StatModType type,object source) : this(value, type, (int)type, source) { }


}
