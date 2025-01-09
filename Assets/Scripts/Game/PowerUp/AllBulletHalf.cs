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

                //foreach(var gun in GunSystem.GunList)
                //{
                //    if (gun is Pistol) continue;
  
                //    var bag = gun.bulletBag;
                //    var bulletCountToAdd = bag.MaxBulletCount / 2;
                //    var gunNeedBulletCount = bag.MaxBulletCount - bag.RemainBulletCount;

                //    if (bulletCountToAdd <= gunNeedBulletCount)
                //    {
                //        bag.RemainBulletCount += bulletCountToAdd;
                //    }
                //    else
                //    {
                //        bag.RemainBulletCount = bag.MaxBulletCount;
                //    }
                //}

                AudioKit.PlaySound("Resources://AllHalfBullet");
                Player.Default.gun.clip.UIReload();
                this.DestroyGameObjGracefully();
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
