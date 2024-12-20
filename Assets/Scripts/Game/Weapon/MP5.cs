using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class MP5 : Gun
	{

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(30);

        public ShootDuration shootDuration = new ShootDuration(0.1f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(60, 60);

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            bulletBag.Reload(clip, reloadSound);
        }

        public void Shoot(Vector2 direction)
        {
            if (clip.CanShoot)
            {
                var bullet = Instantiate(BulletPrefab);
                bullet.transform.position = BulletPrefab.transform.position;
                bullet.velocity = direction.normalized * 25;
                bullet.Damage = Random.Range(1.0f, 2.0f);
                bullet.gameObject.SetActive(true);

                shootLight.ShowLight(BulletPrefab.Position2D(), direction);

                clip.UseBullet();
            }
        }

        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            //Shoot(direction);

            TryPlayShootSound(true);
        }

        public override void Shooting(Vector2 direction)
        {
            if(shootDuration.CanShoot && clip.CanShoot)
            {
                Shoot(direction);
                shootDuration.RecordShootTime();
            }

            if (!clip.CanShoot)
            {
                AudioPlayer.Stop();
            }

        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();
        }

    }
}
