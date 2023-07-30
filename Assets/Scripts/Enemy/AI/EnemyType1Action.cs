using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �ĤH�ƥ�`�I�Ϊ���� ���ƥ�`�I�~�� �ê�l�ƼĤH���ե� 
    /// ����1:���D�ʧ��� ��������~�}�l���� �W�L�l���d��N�|��������^��w�]��m
    /// </summary>
    public class EnemyType1Action : Action
    {
        protected PlayerController player;

        protected Rigidbody2D body;
        protected Animator animator;
        protected EnemyUnitType1 enemyUnitType1;
        protected FacePlayer facePlayer;
        protected OtherCharacterStats selfStats;
        protected string name;
        protected TaskStatus state;

        protected int currentDirection=0;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnitType1 = GetComponent<EnemyUnitType1>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            name = selfStats.enemyBattleData.characterName;
        }
    }
}

