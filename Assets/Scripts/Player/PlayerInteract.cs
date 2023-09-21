using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家與別的物件互動
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    private float interactRange = 0.2f;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && GameManager.Instance.GetCurrentPlayerBehaviourState() == PlayerBehaviourState.None)
        {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null)
            {
                //與可互動對象互動 未來擴充:可以依照互動類型來做分別
                interactable.Interact(transform);
            }
        }
    }
    /// <summary>
    /// 給外部獲得交互對象
    /// </summary>
    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))//找有掛這個腳本的互動
            {
                interactableList.Add(interactable);
            }
        }
        IInteractable closestInteractable = null; //最接近的互動物件
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position)< Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) //比對複數互動物件的距離取近的
                {
                    closestInteractable = interactable;
                }
            }
        }
        return closestInteractable; //回傳最接近的互動物件
    }

    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
