using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    /// <summary>
    /// �p��O�u��ʪ����v�Ǩ�����欰
    /// </summary>
    public class Boss1KeepRandom : Boss1Action
    {
        int randomValue = 0;
        //public override void OnStart()
        //{
        //    randomValue = SetRandom();
        //}
        public override TaskStatus OnUpdate()
        {
            randomValue = SetRandom();

            if (enemyBoss1Unit.isKeepRange) //�O�u�欰
            {
                if (randomValue > 79) //���Z����
                {
                    //�ȵL���Z�����ҥH����
                    state = TaskStatus.Running;
                    return state;
                }
                else if (randomValue > 9 && randomValue < 80) //���k����
                {
                    int randomLR = SetRandom();
                    if (randomLR > 49)
                    {
                        enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.WalkR);
                    }
                    else
                    {
                        enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.WalkL);
                    }
                }
                else //�V���a�a��
                {
                    enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.Near);
                }
                state = TaskStatus.Success;
                return state;
            }
            state = TaskStatus.Running;
            return state;
        }
        private int SetRandom()
        {
            return UnityEngine.Random.Range(0, 101);
        }
    }
}

