/// <summary>
/// �����ݩ�
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
/// �аO����
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
/// ��e�԰�����
/// </summary>
public enum BattleCurrentCharacterNumber
{
    None= 0,
    First = 1,
    Second =2,
    Third =3,
}
/// <summary>
/// ��L����Ϊ������ˮ`������
/// </summary>
public enum CharacterDamageType
{
    Invincible = 0, //�L��
    DestructibleItems = 1, //�i�}�a����
    Player = 2,
    Friendly = 3,
    Enemy = 4,
}
