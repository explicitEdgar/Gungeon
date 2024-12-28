using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class BulletFactory : ViewController
	{
		public static BulletFactory Default;
        private void Awake()
        {
			Default = this;
        }

        void Start()
		{
			// Code Here
		}

        private void OnDestroy()
        {
            Default = null;
        }
    }
}
