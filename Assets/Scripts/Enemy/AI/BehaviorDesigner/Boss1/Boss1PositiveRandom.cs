using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 計算積極行動的機率傳到攻擊行為
    /// </summary>
    public class Boss1PositiveRandom : Boss1Action
    {
        int randomValue = 0;

        //public override void OnStart()
        //{
        //    randomValue = SetRandom();
        //}
        public override TaskStatus OnUpdate()
        {
            randomValue = SetRandom();

            if (enemyBoss1Unit.isPositiveRange) //積極行為
            {
                if (randomValue > 24) //攻擊
                {
                    enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.NormalAttack1);
                }
                else if(randomValue > 4 && randomValue < 25) //左右移動
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
                else //後退
                {
                    enemyBoss1Unit.SetAttackBehavior(EnemyBoss1Unit.Boss1AttackBehavior.BackOff);
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

