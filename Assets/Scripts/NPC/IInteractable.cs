using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���󤬰ʤ����n��@�����e
/// </summary>
public interface IInteractable 
{
    void Interact(Transform interactTransform);
    Transform GetTransform();
    GameObject GetGameObject();
}
