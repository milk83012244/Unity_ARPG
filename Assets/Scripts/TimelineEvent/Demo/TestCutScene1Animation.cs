using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCutScene1Animation : MonoBehaviour
{
    public Animator niru;
    public Animator mo;
    public Animator lia;
    public Animator[] slimes;

    public void PlayWalk()
    {
        niru.Play("Niru_SR_Walk");
        mo.Play("Mo_SR_Walk_Timeline");
        lia.Play("Lia_SR_Walk");
    }
    public void PlayIdle()
    {
        niru.Play("Niru_FrontIdle");
        mo.Play("Mo_FrontIdle");
        lia.Play("Lia_FrontIdle");
    }
    public void PlayNiruAnimation(string name)
    {
        niru.Play(name);
    }
    public void PlayLiaAnimation(string name)
    {
        lia.Play(name);
    }
    public void PlayMoAnimation(string name)
    {
        mo.Play(name);
    }
    public void PlaySlimeWalk()
    {
        for (int i = 0; i < slimes.Length; i++)
        {
            slimes[i].Play("Slime_Blue_SL_Move");
        }
    }
    public void PlaySlimeIdle()
    {
        for (int i = 0; i < slimes.Length; i++)
        {
            slimes[i].Play("Slime_Blue_SL_idle");
        }
    }
}