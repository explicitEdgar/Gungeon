using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace QFramework.Gungeon
{
    public partial class Pistol : Gun
    {

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(15);

        public ShootDuration shootDuration = new ShootDuration(0.4f);

        public ShootLight shootLight = new ShootLight();

        public override BulletBag bulletBag { get; set; } = new BulletBag(-1, -1);

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

                var bullet = Instantiate(BulletPrefab);
                bullet.transform.position = BulletPrefab.transform.position;
                bullet.velocity = direction.normalized * 15;
                bullet.Damage = Random.Range(1.0f, 2.0f);
                bullet.gameObject.SetActive(true);
                
                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();

                shootLight.ShowLight(BulletPrefab.Position2D(), direction);

                clip.UseBullet();
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
