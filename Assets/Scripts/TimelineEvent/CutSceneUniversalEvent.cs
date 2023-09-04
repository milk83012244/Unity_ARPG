using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

/// <summary>
/// Timeline的共用事件方法呼叫
/// </summary>
public class CutSceneUniversalEvent : MonoBehaviour
{
    public Transform playerPosition;

    public UnityEvent CutSceneStartEvent;
    public UnityEvent CutSceneEndEvent;

    public void FollowPlayerPosition()
    {
        this.transform.position = playerPosition.position;
    }

    public void SetPlayerBehaviourStateInCutScene()
    {
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.InCutScene);
        CutSceneStartEvent?.Invoke();
    }
    public void SetPlayerBehaviourStateToNone()
    {
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.None);
        this.gameObject.SetActive(false);
        CutSceneEndEvent?.Invoke();
    }
}
