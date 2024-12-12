using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AWP : Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public ShootDuration shootDuration = new ShootDuration(2f);

        public void Shoot(Vector2 pos, Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = pos;
            bullet.direction = direction.normalized;
            bullet.gameObject.SetActive(true);
        }

        public override void ShootDown(Vector2 direction)
        {
            if (shootDuration.CanShoot)
            {
                shootDuration.RecordShootTime();

                Shoot(Bullet.Position2D(), direction);

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
