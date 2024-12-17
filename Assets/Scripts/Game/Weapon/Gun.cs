using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    [ViewControllerChild]
    public abstract class Gun : ViewController
    {

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public abstract PlayerBullet BulletPrefab { get;  }

        public abstract AudioSource AudioPlayer { get; }

        public virtual void ShootDown(Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = BulletPrefab.transform.position;
            bullet.direction = direction;
            bullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }
        public virtual void Shooting(Vector2 direction)
        {

        }
        public virtual void ShootUp(Vector2 direction)
        {

        }

        public virtual void Reload()
        {
            
        }

    }
}
