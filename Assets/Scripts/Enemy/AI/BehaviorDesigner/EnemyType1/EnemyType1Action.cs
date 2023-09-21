using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �ĤH�欰��ƥ�`�I�Ϊ���� ���ƥ�`�I�~�� �ê�l�ƼĤH���ե� 
    /// ����1:���D�ʧ��� ��������~�}�l���� �W�L�l���d��N�|��������^��w�]��m
    /// </summary>
    public class EnemyType1Action : EnemyPublicAction
    {
        protected EnemyUnitType1 enemyUnitType1;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnitType1 = GetComponent<EnemyUnitType1>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            patrolAI = GetComponent<EnemyType1PartolAI>();
            aIPath = GetComponent<AIPath>();

            attackType = selfStats.enemyBattleData.attackType;

            name = selfStats.enemyBattleData.characterName;
        }
    }
}

