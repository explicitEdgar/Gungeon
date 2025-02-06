using UnityEngine;
using QFramework;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace QFramework.Gungeon
{
	public partial class UIGunList : ViewController, IController
	{
		private static readonly int GrayValue = Shader.PropertyToID("_GrayValue");

		private GunSystem mGunsystem;
        private void Awake()
        {
			mGunsystem = this.GetSystem<GunSystem>();
        }

		

        private void OnEnable()
        {
			Global.UIOpened = true;
			GunItemRoot.DestroyChildren();

			foreach(var gunBaseItem in mGunsystem.GunBaseItems)
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
							Player.DisplayText("<color=yellow>" + cachedItem.Name + "</color>加入战斗!", 2);
							cachedItemView.ButtonUnlock.Hide();
                            cachedItemView.Icon.material.SetFloat(GrayValue, 0);

                            AudioKit.PlaySound("Resources://UnlockGun");

							mGunsystem.Save();
						}
						else
						{
                            Player.DisplayText("你的代币不够",2);
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

        public IArchitecture GetArchitecture()
        {
			return Global.Interface;
        }
    }
}
