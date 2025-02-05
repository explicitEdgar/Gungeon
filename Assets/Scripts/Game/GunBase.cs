using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class GunBase : ViewController
	{
		void Start()
		{
			// Code Here
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                GameUI.Default.UIGunList.Show();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameUI.Default.UIGunList.Hide();
            }
        }
    }
}
