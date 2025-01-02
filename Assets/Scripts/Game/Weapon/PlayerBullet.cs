using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class PlayerBullet : MonoBehaviour
    {
        public Vector2 velocity;

        private Rigidbody2D mRigidbody2D;

        public float Damage { get; set; } = 1;
        private void Awake()
        {
            mRigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            mRigidbody2D.velocity = this.velocity;
        }

        public List<AudioClip> hitEnemySfxs = new List<AudioClip>();
        public List<AudioClip> hitWallSfxs = new List<AudioClip>();

        private void OnCollisionEnter2D(Collision2D other)
        {
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
