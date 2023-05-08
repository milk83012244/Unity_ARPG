
using System.Collections.Generic;
/// <summary>
/// �[���
/// </summary>
public interface IAttack3Observer
{
    void Notify(bool isAttack3);
}
/// <summary>
/// �Q�[���
/// </summary>
public interface IAttack3Subject
{
    /// <summary>
    /// ���U�[���
    /// </summary>
    void RegisterObserver(IAttack3Observer observer);
    /// <summary>
    /// �����[���
    /// </summary>
    void RemoveObserver(IAttack3Observer observer);
    /// <summary>
    /// �q���[���
    /// </summary>
    void NotifyObservers(bool isAttack3);
}

