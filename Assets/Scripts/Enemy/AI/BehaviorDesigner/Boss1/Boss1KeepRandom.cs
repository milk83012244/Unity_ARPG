using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

namespace Sx.EnemyAI
{
    /// <summary>
    /// 計算保守行動的機率傳到攻擊行為
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

            if (enemyBoss1Unit.isKeepRange) //保守行為
            {
                if (randomValue > 79) //遠距攻擊
                {
                    //暫無遠距攻擊所以重骰
                    state = TaskStatus.Running;
                    return state;
                }
                else if (randomValue > 9 && randomValue < 80) //左右移動
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
                else //向玩家靠近
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

