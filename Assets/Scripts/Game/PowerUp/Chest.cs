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
                if (Global.Key.Value > 0)
                {
                    Global.Key.Value--;
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

                    var configs = GunSystem.GetAvailableGuns();

                    if (configs.Count > 0)
                    {
                        var powerUpGun = PowerUpFactory.Default.PowerUpGun.Instantiate()
                            .Position2D(transform.Position2D())
                            .Self(self =>
                            {
                                self.gunConfig = configs.GetRandomItem();
                            })
                            .Show();
                        Room.AddPowerUp(powerUpGun);
                    }
                    else
                    {
                        var hp1 = PowerUpFactory.Default.Hp1.Instantiate()
                            .Position2D(transform.Position2D())
                            .Show();
                        Room.AddPowerUp(hp1);
                    }


                    AudioKit.PlaySound("Resources://Chest");
                    this.DestroyGameObjGracefully();
                }
                else
                {
                    Player.DisplayText("我没有钥匙了");
                }
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
