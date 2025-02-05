using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class GunBase : ViewController
	{
        private bool playerIn = false;
		void Start()
		{
			// Code Here
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                playerIn = true;
                GameUI.Default.UIGunList.Show();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                playerIn = false;
                GameUI.Default.UIGunList.Hide();
            }
        }
    }
}
