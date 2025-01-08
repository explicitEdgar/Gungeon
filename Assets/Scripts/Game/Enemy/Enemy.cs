using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.UIElements;

namespace QFramework.Gungeon
{
    public interface IEnemy
    {
        GameObject GameObject { get; }

        Room Room { get; set; }

        void Hurt(float damage, Vector2 hitDirection);
    }


    public abstract class Enemy : MonoBehaviour,IEnemy
    {
        protected void OnDeath(Vector2 hitDirection,string bodyName,float scale,string soundName = "EnemyDie")
        {
            FxFactory.Default.GeneratoEnemyBody(transform.Position2D(), hitDirection, bodyName, scale);

            AudioKit.PlaySound("Resources://" + soundName);

            var coin = PowerUpFactory.Default.Coin.Instantiate()
                .Position2D(transform.Position2D())
                .Show();

            Room.AddPowerUp(coin);

            Destroy(gameObject);
        }

        public GameObject GameObject => gameObject;

        public Room Room { get; set; }

        public SpriteRenderer SpriteRenderer => gameObject.transform.Find("Sprite").GetComponent<SpriteRenderer>();

        public enum States
        {
            FollowPlayer,
            PrepareShoot,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public Rigidbody2D Rigidbody2D => gameObject.GetComponent<Rigidbody2D>();

        public abstract void Hurt(float damage, Vector2 hitDirection);

        protected void FollowPlayer()
        {
            if (Global.player)
            {
                var direction2Player = (Global.player.transform.position - transform.position).normalized;
                AnimationHelper.UpDownAnimation(SpriteRenderer, 0.05f, State.FrameCountOfCurrentState, 10);
                AnimationHelper.RotateAnimation(SpriteRenderer, 5, State.FrameCountOfCurrentState, 30);
                Rigidbody2D.velocity = direction2Player;

                if (direction2Player.x < 0)
                {
                    SpriteRenderer.flipX = true;
                }
                else
                {
                    SpriteRenderer.flipX = false;
                }
            }
        }
    }
}
