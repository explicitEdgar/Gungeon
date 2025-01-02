using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class ShotGun : QFramework.Gungeon.Gun
	{

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip(8);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(16, 16);

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            bulletBag.Reload(clip, reloadSound);
        }

        public void Shoot(Vector2 pos,Vector2 direction)
        {
            BulletHelper.Shoot(BulletPos.Position2D(), direction, 15, Random.Range(1.0f, 2.0f));
        }

        public ShootDuration shootDuration = new ShootDuration(1f);
        public override void ShootDown(Vector2 direction)
        {   
            if(shootDuration.CanShoot  && clip.CanShoot)
            {
                shootDuration.RecordShootTime();

                var angle = direction.ToAngle();
                var originPos = transform.parent.Position2D();
                var radius = (BulletPos.Position2D() - originPos).magnitude;
                var pos = originPos + radius * direction.normalized;

                var angle1 = angle + 8;
                var direction1 = angle1.AngleToDirection2D();
                var pos1 = originPos + radius * direction1;

                var angle2 = angle - 8;
                var direction2 = angle2.AngleToDirection2D();
                var pos2 = originPos + radius * direction2;

                var angle3 = angle + 4;
                var direction3 = angle3.AngleToDirection2D();
                var pos3 = originPos + radius * direction3;

                var angle4 = angle - 4;
                var direction4 = angle4.AngleToDirection2D();
                var pos4 = originPos + radius * direction4;


                Shoot(pos, direction);
                Shoot(pos1, direction1);
                Shoot(pos2, direction2);
                Shoot(pos3, direction3);
                Shoot(pos4, direction4);

                shootLight.ShowLight(BulletPos.Position2D(), direction);

                clip.UseBullet();

                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.Play();

                CameraController.Shake.Trigger(4, 0.08f);

            }

        }

        public override void Shooting(Vector2 direction)
        {
            ShootDown(direction);

            if (!clip.CanShoot && !AudioPlayer.isPlaying)
            {
                TryPlayEmptySound();
            }
        }
    }
}
