
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
public class Attack3Subject : IAttack3Subject
{
    private List<IAttack3Observer> observers = new List<IAttack3Observer>();

    public void NotifyObservers(bool isAttack3)
    {
        foreach (var observer in observers)
        {
            observer.Notify(isAttack3);
        }
    }

    public void RegisterObserver(IAttack3Observer observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IAttack3Observer observer)
    {
        observers.Remove(observer);
    }
}
