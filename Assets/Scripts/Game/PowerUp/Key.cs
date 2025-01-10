using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Key : ViewController,IPowerUp
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

                Global.Key.Value++;
                AudioKit.PlaySound("Resources://Key");
                this.DestroyGameObjGracefully();
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
