using UnityEngine;
using QFramework;

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
            return enemyName switch
            {
                Constant.EnemyA => Default.EnemyA,
                Constant.EnemyB => Default.EnemyB,
                Constant.EnemyC => Default.EnemyC,
                Constant.EnemyD => Default.EnemyD,
                Constant.EnemyE => Default.EnemyE,
                Constant.EnemyF => Default.EnemyF,
                Constant.EnemyG => Default.EnemyG,
                Constant.EnemyH => Default.EnemyH,
                Constant.EnemyABig => Default.EnemyABig,
                Constant.EnemyBBig => Default.EnemyBBig,
                Constant.EnemyCBig => Default.EnemyCBig,
                Constant.EnemyDBig => Default.EnemyDBig,
                Constant.BossA => Default.BossA,
                Constant.BossB => Default.BossB,
                _ => null
            };
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
            //return Constant.EnemyA;

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
