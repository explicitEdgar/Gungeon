using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    [ViewControllerChild]
    public abstract class Gun : ViewController
    {

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public AudioClip reloadSound;

        public float UnstableRate { get; set; } = 0.1f;

        public virtual PlayerBullet BulletPrefab { get; }

        public abstract AudioSource AudioPlayer { get; }

        public virtual BulletBag bulletBag { get; set; }

        public virtual Clip clip { get; set; }

        public ShootBackForce BackForce = new ShootBackForce();

        public SpriteRenderer Sprite;

        public virtual float GunAddtionSize { get; set; } = 1f;

        public void Start()
        {
            BackForce.Setup(Sprite);
        }

        private void Update()
        {
            BackForce.Update();
        }

        public virtual void ShootDown(Vector2 direction)
        {

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

        public virtual void OnGunUse()
        {
            
        }

        public void TryPlayShootSound(bool loop = false)
        {
            if (!ShootSounds.Contains(AudioPlayer.clip) || !AudioPlayer.isPlaying)
            {
                AudioPlayer.clip = ShootSounds[0];
                AudioPlayer.loop = loop;
                AudioPlayer.Play();
            }
        }

        public void TryPlayEmptySound()
        {
            if (!clip.CanShoot && !clip.reloading)
            {
                AudioPlayer.Stop();

                if (Time.frameCount % 30 == 0)
                {
                    AudioKit.PlaySound("Resources://EmptyBulletSound");
                }
            }
        }

        public GunData Data { get; private set; }

        public void WithData(GunData gunData)
        {
            Data = gunData;
            clip.Data = Data;
            bulletBag.Data = Data;
        }
    }
}
