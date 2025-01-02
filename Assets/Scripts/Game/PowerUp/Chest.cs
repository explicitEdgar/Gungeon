using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Chest : ViewController,IPowerUp
	{   

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Room.PowerUps.Remove(this);

                var hp1 = LevelController.Default.Hp1.Instantiate()
                    .Position2D(transform.Position2D())
                    .Show();
                Room.AddPowerUp(hp1);

                AudioKit.PlaySound("Resources://Chest");
                this.DestroyGameObjGracefully();
            }
        }

        public Room Room { get; set; }
        public SpriteRenderer SpriteRenderer => this.GetComponent<SpriteRenderer>();
    }
}
