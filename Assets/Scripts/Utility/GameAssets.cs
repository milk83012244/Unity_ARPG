using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��������ͦ��� �i�H������n�ͦ�������g�b�o�̦b����a�賣�i�H�����I�s
/// </summary>
public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i ==null)
            {
                _i = (Instantiate(Resources.Load("GameAssets"))as GameObject).GetComponent<GameAssets>(); //�bResources��Ƨ����ؤ@��GameAssets �⦹�}�����W�h 
            }
            return _i;
        }
    }
    //�N�i�H�I�s���b�o�̪�����
    [Header("UI����")]
    public Transform dialogueBubbleCanvas; //��w��ܮ�
}
