using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Live2DAnimationManager : MonoBehaviour
{
    #region 單例
    private Live2DAnimationManager()
    {
        instance = this;
    }

    private static Live2DAnimationManager instance;

    public static Live2DAnimationManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("沒有Live2DAnimationManager實例");
            return instance;
        }
        else
        {
            return instance;
        }
    }
    #endregion

    public GameObject moL2DAnimationObject;
    private Animator moL2DAnimator;
    [HideInInspector] public bool MoUSkillAnimationIsActive; 

    private void Awake()
    {
        moL2DAnimator = moL2DAnimationObject.GetComponent<Animator>();
    }

    public void StartMoUSkillAnimation()
    {
        if (MoUSkillAnimationIsActive == false)
            StartCoroutine(MoUSkillAnimation());
    }
    private IEnumerator MoUSkillAnimation()
    {
        MoUSkillAnimationIsActive = true;
        moL2DAnimationObject.SetActive(true);
        yield return Yielders.GetWaitForSeconds(2f);
        moL2DAnimator.SetTrigger("SetIdle");
        moL2DAnimationObject.SetActive(false);
        MoUSkillAnimationIsActive = false;
    }
}
