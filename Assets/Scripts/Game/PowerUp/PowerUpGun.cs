using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class PowerUpGun : ViewController,IPowerUp
	{
        public GunConfig gunConfig;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                GunSystem.GunList.Add(gunConfig.CreateData());
                Player.Default.UseGun(GunSystem.GunList.Count - 1);

                this.DestroyGameObjGracefully();
            }
        }

        
        private void Start()
        {
            GetComponent<SpriteRenderer>().sprite = Player.Default.GunwithKey(gunConfig.Key).Sprite.sprite;
        }
        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
