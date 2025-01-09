using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace QFramework.Gungeon
{
    public partial class Pistol : Gun
    {

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip(15);

        public ShootDuration shootDuration = new ShootDuration(0.4f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(-1);
       

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            bulletBag.Reload(clip,reloadSound);
        }

        public override void ShootDown(Vector2 direction)
        {   
            if(shootDuration.CanShoot && clip.CanShoot)
            {
                shootDuration.RecordShootTime();

                var angle = direction.ToAngle() + Random.Range(-UnstableRate, UnstableRate) * 30;
                direction = angle.AngleToDirection2D();

                BulletHelper.Shoot(BulletPos.Position2D(), direction, 15, Random.Range(1.0f, 2.0f));
                
                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                shootLight.ShowLight(BulletPos.Position2D(), direction);

                CameraController.Shake.Trigger(2, 0.05f);

                clip.UseBullet();

                BackForce.Shoot(0.05f, 2);
            }

            if (!clip.CanShoot)
            {
                AudioKit.PlaySound("Resources://EmptyBulletSound");
            }
        }
        public override void Shooting(Vector2 direction)
        {

        }
        public override void ShootUp(Vector2 direction)
        {

        }

    }
}
