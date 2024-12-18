using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Bow : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        private Clip clip = new Clip(1);

        public override BulletBag bulletBag { get; set; } = new BulletBag(30, 30);

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
            bullet.direction = direction.normalized;
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

        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            mCurrentScd = 0f;
            isPressing = true;
        }

        public override void Shooting(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            if (isPressing)
            {
                mCurrentScd += Time.deltaTime;
            }

            if (mCurrentScd >= 1f)
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
            if (mCurrentScd >= 1f)
            {
                Shoot(BulletPrefab.Position2D(),direction);
            }
            isPressing = false;
            mCurrentScd = 0f;
            Ready.Hide();
        }
    }
}
