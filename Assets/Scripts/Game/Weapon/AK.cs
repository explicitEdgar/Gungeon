using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AK : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip(30);

        public ShootDuration shootDuration = new ShootDuration(0.2f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(50,50);



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
                bullet.velocity = direction.normalized * 30;
                bullet.Damage = Random.Range(1.5f, 2.5f);
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
            if (shootDuration.CanShoot && clip.CanShoot)
            {
                Shoot(direction);
                shootDuration.RecordShootTime();
            }

            TryPlayEmptySound();
        }

        public override void ShootUp(Vector2 direction)
        {
            if (clip.CanShoot)
            {
                AudioPlayer.Stop();

                AudioPlayer.clip = AKShootEnd;
                AudioPlayer.loop = false;
                AudioPlayer.Play();
            }
        }
    }
}
