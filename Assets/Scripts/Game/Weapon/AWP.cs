using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AWP : Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(10);

        public ShootDuration shootDuration = new ShootDuration(2f);

        private void Start()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            clip.Reload();
        }

        public void Shoot(Vector2 pos, Vector2 direction)
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
            if (shootDuration.CanShoot && clip.CanShoot)
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
