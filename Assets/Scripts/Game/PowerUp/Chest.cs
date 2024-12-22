using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Chest : ViewController
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                LevelController.Default.Hp1.Instantiate()
                    .Position2D(transform.Position2D())
                    .Show();
                AudioKit.PlaySound("Resources://Chest");
                this.DestroyGameObjGracefully();
            }
        }
    }
}
