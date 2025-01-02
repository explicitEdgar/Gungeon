using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Hp1 : ViewController,IPowerUp
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                Global.HP++;
                Global.HPChangedEvent.Invoke();
                AudioKit.PlaySound("Resources://HP1");
                this.DestroyGameObjGracefully();
            }
        }
        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
