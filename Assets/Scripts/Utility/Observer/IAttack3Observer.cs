
using System.Collections.Generic;
/// <summary>
/// 觀察者
/// </summary>
public interface IAttack3Observer
{
    void Notify(bool isAttack3);
}
/// <summary>
/// 被觀察者
/// </summary>
public interface IAttack3Subject
{
    /// <summary>
    /// 註冊觀察者
    /// </summary>
    void RegisterObserver(IAttack3Observer observer);
    /// <summary>
    /// 移除觀察者
    /// </summary>
    void RemoveObserver(IAttack3Observer observer);
    /// <summary>
    /// 通知觀察者
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
