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
                if (Global.HP.Value < Global.MaxHP.Value)
                {
                    Room.PowerUps.Remove(this);

                    Global.HP.Value++;
                    AudioKit.PlaySound("Resources://HP1");
                    this.DestroyGameObjGracefully();
                }
                else
                {
                    Player.DisplayText("现在还不需要");
                }
            }
        }
        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
