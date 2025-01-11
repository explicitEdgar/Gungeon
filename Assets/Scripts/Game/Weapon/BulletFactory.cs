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

        public static void GenBulletShell(Vector2 direction,Rigidbody2D bulletShell = null)
        {   
            if(bulletShell == null)
            {
                bulletShell = Default.PistolShell;
            }

            bulletShell.Instantiate()
                .Position2D(Player.Default.Position2D() + direction * 0.5f)
                .Show()
                .Self(self =>
                {

                    //������ʼ�ٶȺͽ��ٶ�
                    var velocity = -direction * Random.Range(2, 5f) + Vector2.up * Random.Range(3, 6f);
                    var spriteRenderer = self.GetComponent<SpriteRenderer>();
                    self.velocity = velocity;
                    self.angularVelocity = Random.Range(-720, 720);
                    spriteRenderer.sortingLayerName = "Fx";

                    ActionKit.Sequence()
                    .Delay(Random.Range(0.5f, 1), () =>
                    {
                        //��0.5��1s��ص�
                        self.velocity = -direction * Random.Range(0.5f, 2f) + Vector2.up * Random.Range(0, 0.5f);
                        self.gravityScale = 0.1f;
                        self.angularVelocity = RandomUtility.Choose(-1, 1) * Random.Range(180, 720);
                    })
                    .Parallel(p =>
                    {
                        //�ҵ�ʱ������Ч
                        p.PlaySound($"Resources://BulletShell/bullet_shell ({Random.Range(1, 72 + 1)})")
                        //�ص�0.1-0.3s��ֹͣ
                        .Delay(Random.Range(0.1f, 0.3f), () =>
                        {
                            self.angularVelocity = 0;
                            self.gravityScale = 0;
                            self.velocity = Vector2.zero;
                            spriteRenderer.sortingLayerName = "OnFloor";
                        });
                    }).Start(Default);

                    });
        }
    }
}
