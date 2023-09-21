using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���a�P�O�����󤬰�
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
                //�P�i���ʹ�H���� �����X�R:�i�H�̷Ӥ��������Ӱ����O
                interactable.Interact(transform);
            }
        }
    }
    /// <summary>
    /// ���~����o�椬��H
    /// </summary>
    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out IInteractable interactable))//�䦳���o�Ӹ}��������
            {
                interactableList.Add(interactable);
            }
        }
        IInteractable closestInteractable = null; //�̱��񪺤��ʪ���
        foreach (IInteractable interactable in interactableList)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
            }
            else
            {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position)< Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) //���ƼƤ��ʪ��󪺶Z������
                {
                    closestInteractable = interactable;
                }
            }
        }
        return closestInteractable; //�^�ǳ̱��񪺤��ʪ���
    }

    private void OnDrawGizmos()
    {
        UnityEngine.Gizmos.color = UnityEngine.Color.yellow;
        UnityEngine.Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
