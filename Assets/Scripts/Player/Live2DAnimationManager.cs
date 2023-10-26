using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DAnimationManager : MonoBehaviour
{
    #region ³æ¨Ò
    //private Live2DAnimationManager()
    //{
    //    instance = this;
    //}

    private static Live2DAnimationManager instance;
    public static Live2DAnimationManager Instance
    {
        get
        {
            return instance;
        }
    }
    #endregion

    public GameObject moL2DAnimationObject;
    private Animator moL2DAnimator;
    [HideInInspector] public bool MoUSkillAnimationIsActive;
    [HideInInspector] public bool MoUSkillAnimationEnd;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        moL2DAnimator = moL2DAnimationObject.GetComponent<Animator>();
    }

    public void StartMoUSkillAnimation()
    {
        MoUSkillAnimationEnd = false;
        if (MoUSkillAnimationIsActive == false)
            StartCoroutine(MoUSkillAnimation());
    }
    private IEnumerator MoUSkillAnimation()
    {
        GameManager.Instance.SetState(GameState.Paused);
        MoUSkillAnimationIsActive = true;
        moL2DAnimationObject.SetActive(true);
        yield return Yielders.GetWaitForSeconds(2f);
        moL2DAnimator.SetTrigger("SetIdle");
        moL2DAnimationObject.SetActive(false);
        MoUSkillAnimationIsActive = false;

        MoUSkillAnimationEnd = true;
        GameManager.Instance.SetState(GameState.Battle);
    }
}
