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

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(20, 20);


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
            if (clip.CanShoot)
            {
                var bullet = Instantiate(BulletPrefab);
                bullet.transform.position = BulletPrefab.transform.position;
                bullet.direction = direction;
                bullet.Damage = Random.Range(8f, 10f);
                bullet.gameObject.SetActive(true);

                shootLight.ShowLight(BulletPrefab.Position2D(), direction);

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
