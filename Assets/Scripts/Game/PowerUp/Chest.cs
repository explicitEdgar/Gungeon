using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace QFramework.Gungeon
{
	public partial class Chest : ViewController,IPowerUp
	{   

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                //var hp1 = LevelController.Default.Hp1.Instantiate()
                //    .Position2D(transform.Position2D())
                //    .Show();
                //Room.AddPowerUp(hp1);

                //var armor = PowerUpFactory.Default.ArmorDroped.Instantiate()
                //    .Position2D(transform.Position2D())
                //    .Show();
                //Room.AddPowerUp(armor);

                //var halfBullet = PowerUpFactory.Default.AllBulletHalf.Instantiate()
                //    .Position2D(transform.Position2D())
                //    .Show();
                //Room.AddPowerUp(halfBullet);

                //var singleFullBullet = PowerUpFactory.Default.SingleFullBullet.Instantiate()
                //   .Position2D(transform.Position2D())
                //   .Show();
                //Room.AddPowerUp(singleFullBullet);

                var configs = new List<GunConfig>()
                    {
                        GunConfig.ShotGun,
                        GunConfig.MP5,
                        GunConfig.AK,
                        GunConfig.Bow,
                        GunConfig.RocketGun,
                        GunConfig.Laser,
                        GunConfig.AWP
                    };
                configs.RemoveAll(c => GunSystem.GunList.Any(g => g.Key == c.Key));

                if(configs.Count > 0)
                {
                    var gunConfig = configs.GetRandomItem();
                    GunSystem.GunList.Add(gunConfig.CreateData());
                    Player.Default.UseGun(GunSystem.GunList.Count - 1);
                }
                else
                {
                    var hp1 = LevelController.Default.Hp1.Instantiate()
                        .Position2D(transform.Position2D())
                        .Show();
                    Room.AddPowerUp(hp1);
                }


                AudioKit.PlaySound("Resources://Chest");
                this.DestroyGameObjGracefully();
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
