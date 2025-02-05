using UnityEngine;
using QFramework;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace QFramework.Gungeon
{
	public partial class UIGunList : ViewController
	{
		private static readonly int GrayValue = Shader.PropertyToID("_GrayValue");
		public class GunBaseItem
		{
			public string Name;
			public string Key;
			public int Price;
			public bool Unlocked = false;
			public Sprite Icon => Player.Default.GunwithKey(Key).Sprite.sprite;
			public bool InitUnlockState;
		}

		private List<GunBaseItem> mGunBaseItems = new List<GunBaseItem>()
		{
			new() {Name = "ö±µ¯Ç¹",Price = 0,Key = GunConfig.ShotGun.Key,InitUnlockState = true },
			new() {Name = "MP5",Price = 0,Key = GunConfig.MP5.Key,InitUnlockState = true },
			new() {Name = "AK47",Price = 5,Key = GunConfig.AK.Key,InitUnlockState = false },
			new() {Name = "AWP",Price = 6,Key = GunConfig.AWP.Key,InitUnlockState = false },
			new() {Name = "¼¤¹âÇ¹",Price = 7,Key = GunConfig.Laser.Key,InitUnlockState = false },
			new() {Name = "¹­¼ý",Price = 10,Key = GunConfig.Bow.Key,InitUnlockState = false },
			new() {Name = "»ð¼ýÍ²",Price = 15,Key = GunConfig.RocketGun.Key,InitUnlockState = false },
		};

        private void Awake()
        {
            foreach(var gunBaseItem in mGunBaseItems)
			{
				gunBaseItem.Unlocked = PlayerPrefs.GetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.InitUnlockState ? 1 : 0) == 1;
			}
        }

		private void Save()
		{
            foreach (var gunBaseItem in mGunBaseItems)
            {
                PlayerPrefs.SetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.Unlocked ? 1 : 0);
            }
        }

        private void OnEnable()
        {
			Global.UIOpened = true;
			GunItemRoot.DestroyChildren();

			foreach(var gunBaseItem in mGunBaseItems)
			{
				var gunItem = GunItem.InstantiateWithParent(GunItemRoot)
					.Show();

				gunItem.Name.text = gunBaseItem.Name;
				gunItem.ButtonUnlock.gameObject.SetActive(!gunBaseItem.Unlocked);
				gunItem.Icon.sprite = gunBaseItem.Icon;

				if (gunBaseItem.Unlocked)
				{
					gunItem.PriceText.Hide();
					gunItem.ColorIcon.Hide();
					gunItem.Icon.material = gunItem.Icon.material.Instantiate();
					gunItem.Icon.material.SetFloat(GrayValue, 0);
				}
				else
				{
					gunItem.PriceText.text = "x" + gunBaseItem.Price;
                    gunItem.Icon.material = gunItem.Icon.material.Instantiate();
                    gunItem.Icon.material.SetFloat(GrayValue, 1);

                    var cachedItem = gunBaseItem;
					var cachedItemView = gunItem;
					gunItem.ButtonUnlock.onClick.AddListener(() =>
					{
						if (cachedItem.Price <= Global.Color.Value)
						{
							Global.Color.Value -= cachedItem.Price;

							cachedItem.Unlocked = true;
							Player.DisplayText("<color=yellow>" + cachedItem.Name + "</color>¼ÓÈëÕ½¶·!", 2);
							cachedItemView.ButtonUnlock.Hide();
                            cachedItemView.Icon.material.SetFloat(GrayValue, 0);

                            AudioKit.PlaySound("Resources://UnlockGun");

							Save();
						}
						else
						{
                            Player.DisplayText("ÄãµÄ´ú±Ò²»¹»",2);
                        }
                    });
				} 
			}
        }

        void Start()
		{
			// Code Here
		}

        private void OnDisable()
        {
            Global.UIOpened = false;
        }
    }
}
