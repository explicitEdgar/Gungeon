using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class AWP : Gun
	{

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip();

        public ShootDuration shootDuration = new ShootDuration(2f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(20);

        public override float GunAddtionSize => 3f;
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
                BulletHelper.Shoot(BulletPos.Position2D(), direction, 50, Random.Range(8f, 10f));

                shootLight.ShowLight(BulletPos.Position2D(), direction);

                CameraController.Shake.Trigger(5, 0.1f);

                BackForce.Shoot(0.1f, 4);

                clip.UseBullet();

                BulletFactory.GenBulletShell(direction, BulletFactory.Default.AWPShell);
            }
        }

        public override void ShootDown(Vector2 direction)
        {
            if (shootDuration.CanShoot && clip.CanShoot)
            {
                shootDuration.RecordShootTime();

                Shoot(BulletPos.Position2D(), direction);

                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.Play();
            }

        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);

            TryPlayEmptySound();
        }
    }
}
