using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class RocketGun : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(5);

        public ShootDuration shootDuration = new ShootDuration(2f);

        public ShootLight shootLight = new ShootLight();

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            clip.Reload(ReloadSound);
        }

        public void Shoot(Vector2 pos, Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = pos;
            bullet.direction = direction.normalized;
            bullet.gameObject.SetActive(true);
            bullet.transform.right = direction;
        }

        public override void ShootDown(Vector2 direction)
        {
            if (shootDuration.CanShoot  && clip.CanShoot)
            {
                shootDuration.RecordShootTime();

                Shoot(Bullet.Position2D(), direction);

                shootLight.ShowLight(BulletPrefab.Position2D(), direction);

                clip.UseBullet();

                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.Play();
            }

        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);
        }
    }
}
