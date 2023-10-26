using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TestYarnSpinnerAnimator : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    [YarnCommand("set_animation")]
    public void SetAnimation(string name)
    {
        animator.Play(name);
    }
}
