using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class MP5 : Gun
	{

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public ShootDuration shootDuration = new ShootDuration(0.1f);

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
        }

        public override void Shooting(Vector2 direction)
        {
            if(shootDuration.CanShoot)
            {
                Shoot(direction);
                shootDuration.RecordShootTime();
            }

        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();
        }

    }
}
