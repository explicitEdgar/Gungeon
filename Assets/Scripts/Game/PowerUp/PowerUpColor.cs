using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
    public partial class PowerUpColor : ViewController, IPowerUp
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                Global.Color.Value++;
                AudioKit.PlaySound("Resources://Color");
                this.DestroyGameObjGracefully();
            }
        }
        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
