using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Laser : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private bool mShooting = false;

        public ShootDuration shootDuration = new ShootDuration(0.02f);

        public void Shoot(Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = BulletPrefab.transform.position;
            bullet.direction = direction;
            bullet.gameObject.SetActive(true);

        }

        public override void ShootDown(Vector2 direction)
        {
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.loop = true;
            AudioPlayer.Play();

            mShooting = true;
            SelfLineRenderer.enabled = true;
        }

        public override void Shooting(Vector2 direction)
        {
            if (shootDuration.CanShoot)
            {
                Shoot(direction);
            }

            if(mShooting)
            {
                //获得敌人和墙的Layer
                var layers = LayerMask.GetMask("Default", "Enemy");
                //从枪口发射一条物理射线
                var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());
                SelfLineRenderer.SetPosition(1, hit.point);
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            SelfLineRenderer.enabled = false;
            mShooting = false;
        }

    }
}
