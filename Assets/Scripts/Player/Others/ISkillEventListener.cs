using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ޯ��ť�ƥ�
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
