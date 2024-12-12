using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public partial class Pistol : Gun
    {

        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public ShootDuration shootDuration = new ShootDuration(0.4f);

        public override void ShootDown(Vector2 direction)
        {   
            if(shootDuration.CanShoot)
            {
                shootDuration.RecordShootTime();

                var bullet = Instantiate(BulletPrefab);
                bullet.transform.position = BulletPrefab.transform.position;
                bullet.direction = direction;
                bullet.gameObject.SetActive(true);

                var soundIndex = Random.Range(0, ShootSounds.Count);
                AudioPlayer.clip = ShootSounds[soundIndex];
                AudioPlayer.Play();
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
