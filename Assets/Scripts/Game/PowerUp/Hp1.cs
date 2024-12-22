using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Hp1 : ViewController
	{
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Global.HP++;
                Global.HPChangedEvent.Invoke();
                AudioKit.PlaySound("Resources://HP1");
                this.DestroyGameObjGracefully();
            }
        }
    }
}
