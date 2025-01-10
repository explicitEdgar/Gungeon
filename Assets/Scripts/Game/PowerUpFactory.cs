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
