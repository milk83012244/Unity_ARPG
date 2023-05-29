using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �ĤH�ƥ�`�I�Ϊ���� ���ƥ�`�I�~��
    /// </summary>
    public class EnemyAction : Action
    {
        protected PlayerController player;

        protected Rigidbody2D body;
        protected Animator animator;
        protected EnemyUnits enemyUnits;
        protected FacePlayer facePlayer;
        protected OtherCharacterStats selfStats;
        protected TaskStatus state;

        protected int currentDirection=0;

        public override void OnAwake()
        {
            player = PlayerController.GetInstance();

            body = GetComponent<Rigidbody2D>();
            enemyUnits = GetComponent<EnemyUnits>();
            facePlayer = GetComponent<FacePlayer>();
            selfStats = GetComponent<OtherCharacterStats>();
            animator = this.gameObject.GetComponentInChildren<Animator>();
        }
    }
}

