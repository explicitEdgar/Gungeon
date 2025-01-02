using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class PowerUpFactory : ViewController
	{
		public static PowerUpFactory Default;

        private void Awake()
        {
			Default = this;
        }

        private void OnDestroy()
        {
            Default = null;
        }

        void Start()
		{
			// Code Here
		}
	}
}
