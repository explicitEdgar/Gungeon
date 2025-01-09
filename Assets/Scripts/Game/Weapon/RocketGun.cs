using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class RocketGun : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip(5);

        public ShootDuration shootDuration = new ShootDuration(2f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(10);

        public override float GunAddtionSize => 2f;

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            bulletBag.Reload(clip, reloadSound);
        }

        public void Shoot(Vector2 pos, Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = pos;
            bullet.velocity = direction.normalized * 5;
            bullet.Damage = Random.Range(5f, 10f);
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

                CameraController.Shake.Trigger(5, 0.1f);

                BackForce.Shoot(0.08f, 3);

            }

        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);

            if (!clip.CanShoot && !AudioPlayer.isPlaying)
            {
                TryPlayEmptySound();
            }
        }
    }
}
