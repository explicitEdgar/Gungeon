using UnityEngine;
using QFramework;
using System;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class PowerUpFactory : ViewController
	{
		public static PowerUpFactory Default;

        internal static void GeneratePowerUp(Enemy enemy)
        {
            if (enemy.isBoss)
            {
                // 颜色生成
                var colorCount = UnityEngine.Random.Range(3, 5 + 1);

                for (int i = 0; i < colorCount; i++)
                {
                    var angle = UnityEngine.Random.Range(0, 360);
                    var powerUp = PowerUpFactory.Default.PowerUpColor
                       .Instantiate()
                       .Position2D(enemy.GameObject.Position2D() +
                            angle.AngleToDirection2D() * UnityEngine.Random.Range(0.5f, 1))
                       .LocalPositionZ(0)
                       .Show();
                    enemy.Room.AddPowerUp(powerUp);
                }

                // 补给生成
                var powerUps = new List<IPowerUp>()
                {
                    Default.Hp1,
                    Default.Hp1,
                    Default.ArmorDroped,
                    Default.ArmorDroped,
                    Default.SingleFullBullet,
                    Default.AllBulletHalf,
                };

                var takeCount = UnityEngine.Random.Range(1, 1 + 3);
                for(int i = 0;i < takeCount;i++)
                {
                    var angle = UnityEngine.Random.Range(0, 360);
                    var powerUp = powerUps.GetAndRemoveRandomItem()
                        .SpriteRenderer.gameObject
                       .Instantiate()
                       .Position2D(enemy.GameObject.Position2D() +
                            angle.AngleToDirection2D() * UnityEngine.Random.Range(0.5f, 1))
                       .LocalPositionZ(0)
                       .Show();
                    enemy.Room.AddPowerUp(powerUp.GetComponent<IPowerUp>());
                }

                return;
            }

            var list = new List<IPowerUp>();
            if(Global.HP.Value < 6)
            {
                if(UnityEngine.Random.Range(0,100) < 20)
                {
                    list.Add(Default.Hp1);
                }
                else if(UnityEngine.Random.Range(0, 100) < 5)
                {
                    list.Add(Default.Hp1);
                }
            }

            if(UnityEngine.Random.Range(0, 100) < 10)
            {
                list.Add(Default.ArmorDroped);
            }

            if(UnityEngine.Random.Range(0, 100) < 50)
            {
                list.Add(Default.Coin);
            }

            if(list.Count > 0)
            {
                var angle = UnityEngine.Random.Range(0, 360);
                var dropThing = list.GetRandomItem().SpriteRenderer
                    .Instantiate()
                    .Position2D(enemy.transform.Position2D() + angle.AngleToDirection2D() * UnityEngine.Random.Range(0.25f,0.5f))
                    .Show();

                enemy.Room.AddPowerUp(dropThing.GetComponent<IPowerUp>());
            }
            
        }

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
	}
}
