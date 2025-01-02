using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class MP5 : Gun
	{


        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip(30);

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
                BulletHelper.Shoot(BulletPos.Position2D(), direction, 25, Random.Range(1.0f, 2.0f));

                shootLight.ShowLight(BulletPos.Position2D(), direction);

                CameraController.Shake.Trigger(2, 0.05f);


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
                TryPlayShootSound(true);
            }

            TryPlayEmptySound();

        }

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();
        }

    }
}
