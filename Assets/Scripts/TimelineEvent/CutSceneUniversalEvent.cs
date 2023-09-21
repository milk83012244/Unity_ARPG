using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

/// <summary>
/// Timeline���@�Ψƥ��k�I�s
/// </summary>
public class CutSceneUniversalEvent : MonoBehaviour
{
    public UnityAction CutSceneStartEvent;
    public UnityAction CutSceneEndEvent;

    public GameObject playerObj;
    public GameObject MainUICanvas;

    public void FollowPlayerPosition()
    {
        this.transform.position = playerObj.transform.position;
    }
    public void PlayerFollowPosition(GameObject gameObject)
    {
        playerObj.transform.position = gameObject.transform.position;
    }

    public void SetPlayerBehaviourStateInCutScene()
    {
        ActiveMainUI(false);
        ActivePlayerGameObject(false);
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.InCutScene);
        CutSceneStartEvent?.Invoke();
    }
    public void SetPlayerBehaviourStateToNone()
    {
        ActiveMainUI(true);
        ActivePlayerGameObject(true);
        GameManager.Instance.SetPlayerBehaviourState(PlayerBehaviourState.None);
        this.gameObject.SetActive(false);
        CutSceneEndEvent?.Invoke();
    }
    public void SetGameState(int gameState)
    {
        if (playerObj.activeSelf)
        {
            GameManager.Instance.SetState((GameState)gameState);
        }
    }
    public void ActiveMainUI(bool isActive)
    {
        MainUICanvas.SetActive(isActive);
    }
    public void ActivePlayerGameObject(bool isActive)
    {
        playerObj.SetActive(isActive);
    }
}