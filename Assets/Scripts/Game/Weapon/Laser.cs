using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Laser : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(100);

        private bool mShooting = false;

        public ShootDuration shootDuration = new ShootDuration(0.02f);

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            clip.Reload(ReloadSound);
        }

        public void Shoot(Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = BulletPrefab.transform.position;
            bullet.direction = direction;
            bullet.gameObject.SetActive(true);

            clip.UseBullet();

        }

        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            Shoot(direction);

            TryPlayShootSound(true);

            mShooting = true;
            SelfLineRenderer.enabled = true;
        }

        public override void Shooting(Vector2 direction)
        {
            if (clip.CanShoot)
            {
                if (shootDuration.CanShoot)
                {
                    Shoot(direction);
                }

                if (mShooting)
                {
                    //获得敌人和墙的Layer
                    var layers = LayerMask.GetMask("Default", "Enemy");
                    //从枪口发射一条物理射线
                    var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                    SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());
                    SelfLineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                AudioPlayer.Stop();

                SelfLineRenderer.enabled = false;
                mShooting = false;
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
