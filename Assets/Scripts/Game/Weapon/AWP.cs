using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AWP : Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public void Shoot(Vector2 pos, Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = pos;
            bullet.direction = direction.normalized;
            bullet.gameObject.SetActive(true);
        }

        private float mlastShootTime = 0;
        private float ShootDuration = 2f;
        public override void ShootDown(Vector2 direction)
        {
            if (Time.time - mlastShootTime >= ShootDuration || mlastShootTime == 0)
            {
                mlastShootTime = Time.time;

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
