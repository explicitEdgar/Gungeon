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

        private void Start()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            clip.Reload();
        }

        public void Shoot(Vector2 direction)
        {
            if (clip.CanShoot)
            {
                var bullet = Instantiate(BulletPrefab);
                bullet.transform.position = BulletPrefab.transform.position;
                bullet.direction = direction;
                bullet.gameObject.SetActive(true);

                clip.UseBullet();
            }
        }

        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            Shoot(direction);

            AudioPlayer.clip = ShootSounds[0];
            AudioPlayer.loop = true;
            AudioPlayer.Play();
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
