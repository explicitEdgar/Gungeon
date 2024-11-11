using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public partial class Pistol : ViewController
    {

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public void ShootDown(Vector2 direction)
        {
            var bullet = Instantiate(Bullet);
            bullet.transform.position = Bullet.transform.position;
            bullet.direction = direction;
            bullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            SelfAudioSource.clip = ShootSounds[soundIndex];
            SelfAudioSource.Play();
        }
        public void Shooting(Vector2 direction)
        {

        }
        public void ShootUp(Vector2 direction)
        {

        }
    }
}
