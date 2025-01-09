using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AllBulletHalf : ViewController,IPowerUp
	{
        void Start()
        {
            // Code Here
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                foreach (var gun in GunSystem.GunList)
                {
                    if (gun.Key == GunConfig.Pistol.Key) continue;

                    var bulletCountToAdd = gun.Config.GunBagMaxBulletCount / 2;
                    var gunNeedBulletCount = gun.Config.GunBagMaxBulletCount - gun.GunBagRemainBulletCount;

                    if (bulletCountToAdd <= gunNeedBulletCount)
                    {
                        gun.GunBagRemainBulletCount += bulletCountToAdd;
                    }
                    else
                    {
                        gun.GunBagRemainBulletCount = gun.Config.GunBagMaxBulletCount;
                    }
                }

                AudioKit.PlaySound("Resources://AllHalfBullet");
                Player.Default.gun.clip.UIReload();
                this.DestroyGameObjGracefully();
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
