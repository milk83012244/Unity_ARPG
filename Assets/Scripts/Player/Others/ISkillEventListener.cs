using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 技能監聽事件
/// </summary>
public interface ISkillEventListener
{
    void OnSkillEventOccurred();
    void OnSkillEventEnd();
}
public class EventSender
{
    public event Action SkillEventOccurred;
    public event Action SkillEventEnd;

    public void TriggerOccurredEvent()
    {
        SkillEventOccurred?.Invoke();
    }
    public void TriggerEndEvent()
    {
        SkillEventEnd?.Invoke();
    }
}
