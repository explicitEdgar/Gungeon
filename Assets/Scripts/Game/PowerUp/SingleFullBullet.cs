using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class SingleFullBullet : ViewController,IPowerUp
	{
        void Start()
        {
            // Code Here
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var gun = Player.Default.gun;
                if (gun is Pistol || gun.bulletBag.Full)
                {

                }
                else
                {
                    Room.PowerUps.Remove(this);

                    gun.bulletBag.Data.GunBagRemainBulletCount = gun.bulletBag.MaxBulletCount;

                    AudioKit.PlaySound("Resources://AllHalfBullet");
                    Player.Default.gun.clip.UIReload();
                    this.DestroyGameObjGracefully();
                }
               
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
