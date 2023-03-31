/// <summary>
/// �ƭȭק�����
/// </summary>
public enum StatModType
{
    Flat = 100,//���
    PercentAdd = 200,//�|�[�ʤ���
    PercentMult = 300,//���v�W�[�ʤ���
}
/// <summary>
/// �ƭȭקﾹ(�Ҧp�����O�K�[ BUFF �˳� ���~��)
/// </summary>
public class StatModifier
{
    /// <summary>
    /// �קﾹ����
    /// </summary>
    public readonly float Value;

    public readonly StatModType Type;

    /// <summary>
    /// �u����
    /// </summary>
    public readonly int Order;

    /// <summary>
    /// �����Ҧ������ӷ�
    /// </summary>
    public readonly object Source;

    /// <summary>
    /// ��l��
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
