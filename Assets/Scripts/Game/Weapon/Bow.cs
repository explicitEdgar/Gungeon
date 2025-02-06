using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace QFramework.Gungeon
{
	public partial class Bow : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public List<AudioClip> PrepareSounds = new List<AudioClip>();

        public override Clip clip { get; set; } = new Clip();

        public override BulletBag bulletBag { get; set; } = new BulletBag(30);

        public float needTime = 0.5f;

        public override float GunAddtionSize => 1.5f;

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
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = pos;
            bullet.velocity = direction.normalized * 30;
            bullet.Damage = Random.Range(3f, 5f);
            bullet.gameObject.SetActive(true);
            bullet.transform.right = direction;

            clip.UseBullet();

            var soundIndex = Random.Range(0, ShootSounds.Count);
            AudioPlayer.clip = ShootSounds[soundIndex];
            AudioPlayer.Play();
        }

        private float mCurrentScd = 0f;
        private bool isPressing = false;

        private AudioPlayer mPullBowPlayer = null;
        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot)
            {
                TryPlayEmptySound();
                return;
            }
            mCurrentScd = 0f;
            isPressing = true;
            mPullBowPlayer = AudioKit.PlaySound(PrepareSounds.GetRandomItem(),callBack:(_) =>
            {
                mPullBowPlayer = null;
            });
        }

        public override void Shooting(Vector2 direction)
        {
            if (!clip.CanShoot)
            {
                return;
            }
                if (isPressing)
            {
                mCurrentScd += Time.deltaTime;
            }

            if (mCurrentScd >= needTime)
            {
                Ready.Show();
            }
            else
            {
                Ready.Hide();
            }
        }

        public override void ShootUp(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            if (mCurrentScd >= needTime)
            {
                Shoot(BulletPrefab.Position2D(),direction);
            }
            else
            {
                if(mPullBowPlayer != null)
                {
                    mPullBowPlayer.Stop();
                    mPullBowPlayer = null;
                }
            }
            isPressing = false;
            mCurrentScd = 0f;
            Ready.Hide();
        }
    }
}
