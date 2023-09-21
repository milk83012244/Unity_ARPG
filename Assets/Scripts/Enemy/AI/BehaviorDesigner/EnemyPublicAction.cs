using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using Pathfinding;


namespace Sx.EnemyAI
{
    /// <summary>
    /// �ĤH�欰��ƥ�@�θ��
    /// </summary>
    public class EnemyPublicAction : Action
    {
        //�M������
        protected EnemyType1PartolAI patrolAI; //����
        protected AIDestinationSetter aiDestinationSetter; //���w�ؼ�
        protected AIPath aIPath; //���u����

        protected PlayerController player;
        protected Rigidbody2D body;
        protected Animator animator;
        protected FacePlayer facePlayer;
        protected OtherCharacterStats selfStats;
        protected string name;
        protected TaskStatus state;
        protected AttackType attackType; //��������

        protected int currentDirection = 0;

        /// <summary>
        /// �}��AI�M�����ʥ\��ö}�l�j�M���|
        /// </summary>
        public virtual void StartAIPath()
        {
            aIPath.isStopped = false;
            aIPath.canSearch = true;
        }
        /// <summary>
        /// ����AI�M�����ʥ\��B����j�M���|
        /// </summary>
        public virtual void StopAIPath()
        {
            aIPath.isStopped = true;
            aIPath.canSearch = false;
        }
    }
}


