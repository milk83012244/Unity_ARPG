/// <summary>
/// 元素屬性
/// </summary>
public enum ElementType
{
    Fire,
    Ice,
    Wind,
    Thunder,
    Light,
    Dark,
    None =99,
}
/// <summary>
/// 標記類型
/// </summary>
public enum MarkType
{
    Mo,
    None = 99,
}
public enum AttackType
{
    Melee = 0,
    RangedAttack,
    Special,
}
/// <summary>
/// 當前戰鬥角色
/// </summary>
public enum BattleCurrentCharacterNumber
{
    None= 0,
    First = 1,
    Second =2,
    Third =3,
}
/// <summary>
/// 其他角色或物件受到傷害的類型
/// </summary>
public enum CharacterDamageType
{
    Invincible = 0, //無敵
    DestructibleItems = 1, //可破壞物件
    Player = 2,
    Friendly = 3,
    Enemy = 4,
}
