using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;

namespace Sx.EnemyAI
{
    /// <summary>
    /// Boss1:Boss�Ь��T�w�Ŷ� ���d�򤺳��|�D�ʧ���,���O�����a�W�L�@�w�Z���|�i�J�O�u�Ҧ�
    /// �欰:�i�J�����d���,�|�H���׶}�άO�o�ʧ���,�O�u�Ҧ��ɵo�ʧ������W�v�|����C
    /// </summary>
    public class Boss1Action : EnemyPublicAction
    {
        protected EnemyBoss1Unit enemyBoss1Unit;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();
            body = GetComponent<Rigidbody2D>();
            enemyBoss1Unit = GetComponent<EnemyBoss1Unit>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();

            aiDestinationSetter = GetComponent<AIDestinationSetter>();
            //patrolAI = GetComponent<EnemyType1PartolAI>();
            aIPath = GetComponent<AIPath>();

            attackType = selfStats.enemyBattleData.attackType;

            name = selfStats.enemyBattleData.characterName;
        }

        protected virtual void StopAIMove()
        {
            aIPath.canMove = false;
        }
        protected virtual void StartAIMove()
        {
            aIPath.canMove = true;
        }
    }
}

