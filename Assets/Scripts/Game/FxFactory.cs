using UnityEngine;
using QFramework;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace QFramework.Gungeon
{
	public partial class FxFactory : ViewController
	{
		public static FxFactory Default;
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

        public void GenerateHurtFx(Vector2 pos,Color color = default)
        {
            if (color == default) color = Color.red;
            FxFactory.Default.HurtFx
                    .Instantiate()
                    .Position2D(pos)
                    .Show()
                    .Self(self =>
                    {
                        var main = self.main;
                        main.startColor = color;
                        ActionKit.Delay(self.main.duration + 0.3f, self.DestroyGameObjGracefully).StartCurrentScene();
                    })
                    .Play();
        }

        public void GeneratePlayerBlood(Vector2 originPos)
        {
            var blood = FxFactory.Default.PlayerBlood.Instantiate()
                .Position2D(originPos)
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360);
            var radius = Random.Range(0.2f, 1.5f);
            var moveBy = angle.AngleToDirection2D() * radius;
            var scaleTo = Random.Range(0.2f, 3.0f);

            ActionKit.Lerp(0, 1, Random.Range(0.1f, 0.3f), (p) =>
            {
                p = EaseUtility.InCubic(0, 1, p);
                blood.Position2D(bloodOriginPos + moveBy * p);
                blood.LocalScale(scaleTo * p);
            }).StartCurrentScene();
        }

        public void GenerateEnemyBlood(Vector2 originPos)
        {
            var blood = FxFactory.Default.EnemyBlood.Instantiate()
                .Position2D(originPos)
                .EulerAnglesZ(Random.Range(0, 360f))
                .LocalScale(0.1f)
                .Show();

            var bloodOriginPos = blood.Position2D();
            var angle = Random.Range(0, 360);
            var radius = Random.Range(0.2f, 1.5f);
            var moveBy = angle.AngleToDirection2D() * radius;
            var scaleTo = Random.Range(0.2f, 3.0f);

            ActionKit.Lerp(0, 1, Random.Range(0.1f, 0.3f), (p) =>
            {
                p = EaseUtility.InCubic(0, 1, p);
                blood.Position2D(bloodOriginPos + moveBy * p);
                blood.LocalScale(scaleTo * p);
            }).StartCurrentScene();
        }
    }
}
