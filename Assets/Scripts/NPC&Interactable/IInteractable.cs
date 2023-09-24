using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件互動介面要實作的內容
/// </summary>
public interface IInteractable 
{
    void Interact(Transform interactTransform);
    Transform GetTransform();
    GameObject GetGameObject();
}
