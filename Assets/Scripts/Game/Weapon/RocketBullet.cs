using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class RocketBullet : PlayerBullet
    {

        protected override void OnCollisionEnter2D(Collision2D other)
        {
            BulletFactory.Default.Explosion
               .Instantiate()
               .Position2D(transform.Position2D())
               .Show();

            if (other.gameObject.CompareTag("Enemy"))
            {
                this.Hide();
                var enemy = other.gameObject.GetComponent<IEnemy>();
                enemy.Hurt(Damage,-other.GetContact(0).relativeVelocity.normalized);
                if (hitEnemySfxs.Count > 0)
                {
                    var hitEnemySfx = hitEnemySfxs.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitEnemySfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    }, volume: 0.3f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }
            }
            else if(other.gameObject.CompareTag("Wall"))
            {
                Debug.Log(0);
                this.Hide();
                if (hitWallSfxs.Count > 0)
                {
                    var hitWallSfx = hitWallSfxs.GetRandomItem();
                    var audioPlayer = AudioKit.PlaySound(hitWallSfx, callBack: (_) =>
                    {
                        this.DestroyGameObjGracefully();
                    }, volume: 0.3f);
                }
                else
                {
                    this.DestroyGameObjGracefully();
                }
            }
        }
    }
}
