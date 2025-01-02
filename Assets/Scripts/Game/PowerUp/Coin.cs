using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Coin : ViewController
	{
		void Start()
		{
			// Code Here
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Global.Coin.Value++;
                AudioKit.PlaySound("Resources://Coin");
                this.DestroyGameObjGracefully();
            }
        }
    }
}
