using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �ĤH�欰��ƥ�`�I�Ϊ���� ���ƥ�`�I�~�� �ê�l�ƼĤH���ե� 
    /// ����2:���a�b���Ľd�򤺥D�ʧ��� �W�L�l���d��N�|��������^��w�]��m
    /// </summary>
    public class EnemyType2Action : EnemyPublicAction
    {
        protected EnemyUnitType2 enemyUnitType2;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnitType2 = GetComponent<EnemyUnitType2>();
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
