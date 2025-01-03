using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class ShopItem : ViewController
	{   
        public Room Room { get; set; }
        public IPowerUp PowerUp { get; set; }

        public int ItemPrice { get; set; }

        public ShopItem UpdateView()
        {
            Price.text = $"${ItemPrice}";
            Icon.sprite = PowerUp.SpriteRenderer.sprite;

            return this;
        }
		void Start()
		{
			// Code Here
		}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Tip.Show();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Tip.text = "(F)";
                Tip.Hide();
            }
        }

        private void Update()
        {
            if(Tip.gameObject.activeSelf)
            {
                if(Input.GetKeyDown(KeyCode.F) && Global.CanDo)
                {
                    if (Global.Coin.Value >= ItemPrice)
                    {
                        Global.Coin.Value -= ItemPrice;

                        var powerUp = PowerUp.SpriteRenderer.Instantiate()
                            .Position2D(transform.Position2D())
                            .Show();

                        Room.AddPowerUp(powerUp.GetComponent<IPowerUp>());

                        this.DestroyGameObjGracefully();
                    }
                    else
                    {
                        Tip.text = "½ð±Ò²»×ã";
                        AudioKit.PlaySound("Resources://Warning");
                    }
                }
            }
        }
    }
}
