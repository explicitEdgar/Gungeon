using UnityEngine;
using QFramework;
using UnityEditor.Playables;

namespace QFramework.Gungeon
{
	public partial class EnemyFactory : ViewController
	{
		public static EnemyFactory Default;

        private void Awake()
        {
			Default = this;
        }
        private void OnDestroy()
        {
            Default = null;
        }
        void Start()
		{
			// Code Here
		}

        public static IEnemy EnemyByName(string enemyName)
        {
            if(enemyName == Constant.EnemyA)
            {
                return Default.EnemyA;
            }
            else if(enemyName == Constant.EnemyB)
            {
                return Default.EnemyB;
            }
            else if (enemyName == Constant.EnemyC)
            {
                return Default.EnemyC;
            }
            else if (enemyName == Constant.EnemyD)
            {
                return Default.EnemyD;
            }
            else if (enemyName == Constant.EnemyE)
            {
                return Default.EnemyE;
            }
            else if (enemyName == Constant.EnemyF)
            {
                return Default.EnemyF;
            }
            else if (enemyName == Constant.EnemyG)
            {
                return Default.EnemyG;
            }
            else if (enemyName == Constant.EnemyH)
            {
                return Default.EnemyH;
            }
            else if (enemyName == Constant.EnemyABig)
            {
                return Default.EnemyABig;
            }
            else if (enemyName == Constant.EnemyBBig)
            {
                return Default.EnemyBBig;
            }
            else if (enemyName == Constant.EnemyCBig)
            {
                return Default.EnemyCBig;
            }
            else if (enemyName == Constant.EnemyDBig)
            {
                return Default.EnemyDBig;
            }
            else
            {
                return null;
            }
        }

        public static int GenTargetEnemyScore()
        {
            if(Level1.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(2, 2, 3, 3, 8);
            }
            else if(Level1.Config == Global.CurrentLevel)
            {
                return RandomUtility.Choose(2, 2, 3, 3,4,4, 8,9);
            }

            return RandomUtility.Choose(2, 3, 4, 5, 6, 7, 8, 9,10);
        }
        public static string EnemyNameByScore(int score)
        {
            if(score == 2)
            {
                return Constant.EnemyA;
            }
            else if (score == 3)
            {
                return RandomUtility.Choose(Constant.EnemyB,Constant.EnemyC);
            }
            else if (score == 4)
            {
                return RandomUtility.Choose(Constant.EnemyD, Constant.EnemyE);
            }
            else if (score == 5)
            {
                return Constant.EnemyF;
            }
            else if (score == 6)
            {
                return Constant.EnemyG;
            }
            else if(score == 7)
            {
                return Constant.EnemyH;
            }
            else if (score == 8)
            {
                return Constant.EnemyABig;
            }
            else if (score == 9)
            {
                return RandomUtility.Choose(Constant.EnemyBBig, Constant.EnemyCBig);
            }
            else if (score == 10)
            {
                return Constant.EnemyDBig;
            }
            else
            {
                return null;
            }

        }
	}
}
