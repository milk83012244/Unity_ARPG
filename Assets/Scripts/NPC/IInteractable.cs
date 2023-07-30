using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���󤬰ʤ����n��@�����e
/// </summary>
public interface IInteractable 
{
    void Interact(Transform interactTransform,bool Dialogue=false);
    Transform GetTransform();
    GameObject GetGameObject();
}
