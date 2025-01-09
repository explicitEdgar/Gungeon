using UnityEngine;
using QFramework;

namespace QFramework.Gungeon
{
	public partial class Laser : QFramework.Gungeon.Gun
	{
        public override PlayerBullet BulletPrefab => Bullet;

        public override AudioSource AudioPlayer => SelfAudioSource;

        public override Clip clip { get; set; } = new Clip();

        private bool mShooting = false;

        public ShootDuration shootDuration = new ShootDuration(0.02f);

        public override BulletBag bulletBag { get; set; } = new BulletBag(2000);

        public override float GunAddtionSize => 1.5f;

        public override void OnGunUse()
        {
            clip.UIReload();
        }

        public override void Reload()
        {
            bulletBag.Reload(clip, reloadSound);
        }

        public void Shoot(Vector2 direction)
        {
            var bullet = Instantiate(BulletPrefab);
            bullet.transform.position = mLaserHitPoint;
            bullet.velocity = direction.normalized * 100;
            bullet.Damage = 1f;
            bullet.gameObject.SetActive(true);

            clip.UseBullet();

        }

        public override void ShootDown(Vector2 direction)
        {
            if (!clip.CanShoot) return;
            //Shoot(direction);

            TryPlayShootSound(true);

            mShooting = true;
            SelfLineRenderer.enabled = true;
        }

        public override void Shooting(Vector2 direction)
        {
            if (clip.CanShoot)
            {
                if (shootDuration.CanShoot)
                {
                    Shoot(direction);
                    TryPlayShootSound(true);
                    mShooting = true;
                    SelfLineRenderer.enabled = true;
                }

                if (mShooting)
                {
                    //��õ��˺�ǽ��Layer
                    var layers = LayerMask.GetMask("Default", "Enemy","Wall");
                    //��ǹ�ڷ���һ����������
                    var hit = Physics2D.Raycast(BulletPrefab.Position2D(), direction, float.MaxValue, layers);
                    mLaserHitPoint = hit.point;
                    SelfLineRenderer.SetPosition(0, BulletPrefab.Position2D());
                    SelfLineRenderer.SetPosition(1, hit.point);
                }
            }
            else
            {
                TryPlayEmptySound();
                mShooting = false;
                SelfLineRenderer.enabled = false;
            }
        }

        Vector2 mLaserHitPoint = Vector2.zero;

        public override void ShootUp(Vector2 direction)
        {
            AudioPlayer.Stop();

            SelfLineRenderer.enabled = false;
            mShooting = false;
        }

    }
}
