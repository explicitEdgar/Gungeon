using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class MP5 : ViewController
	{

        public PlayerBullet playerBullet;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public AudioSource ShootSoundPlayer;

        public void ShootDown(Vector2 direction)
        {
            var bullet = Instantiate(playerBullet);
            bullet.transform.position = playerBullet.transform.position;
            bullet.direction = direction;
            bullet.gameObject.SetActive(true);

            var soundIndex = Random.Range(0, ShootSounds.Count);
            ShootSoundPlayer.clip = ShootSounds[soundIndex];
            ShootSoundPlayer.Play();
        }
        public void Shooting(Vector2 direction)
        {

        }
        public void ShootUp(Vector2 direction)
        {

        }
    }
}
