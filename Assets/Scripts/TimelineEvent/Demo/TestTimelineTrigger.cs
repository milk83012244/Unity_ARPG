using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DemoTrigger : MonoBehaviour
{
    public Transform TeleportPoint;
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Ĳ�o�ǰe");
        //��oPlayer���I���� ���~�ǰe
        //�ǰe �H�J�H�X�ĪG
        this.gameObject.SetActive(false);
    }
}
