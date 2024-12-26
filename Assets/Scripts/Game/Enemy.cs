using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{   
    public interface IEnemy
    {
        GameObject GameObject { get; }
    }


    public class Enemy : MonoBehaviour, IEnemy
    {
        public GameObject GameObject => gameObject;

        public Player player;

        public EnemyBullet enemyBullet;

        public Rigidbody2D mrigidbody2D;

        public enum States
        {
            FollowPlayer,
            Shoot,
        }

        public FSM<States> State = new FSM<States>();

        public float followPlayerScd = 3.0f;

        public float shootScd = 1.0f;

        public SpriteRenderer spriteRenderer;

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        private float Hp = 3;


        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
        }

        public void Awake()
        {
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    followPlayerScd = Random.Range(1.0f, 4.0f);
                })
                .OnUpdate(() =>
                {
                    if (Global.player)
                    {
                        var direction2Player = (Global.player.transform.position - transform.position).normalized;

                        mrigidbody2D.velocity = direction2Player;

                        if (direction2Player.x < 0)
                        {
                            spriteRenderer.flipX = true;
                        }
                        else
                        {
                            spriteRenderer.flipX = false;
                        }
                    }

                    if (State.SecondsOfCurrentState >= followPlayerScd)
                    {
                        State.ChangeState(States.Shoot);
                    }
                });

            State.State(States.Shoot)
                .OnEnter(() =>
                {
                    if (Global.player)
                    {
                        mrigidbody2D.velocity = new Vector2(0, 0);
                        var bullet = Instantiate(enemyBullet);
                        bullet.transform.position = transform.position;
                        var direction2Player = (Global.player.transform.position - transform.position).normalized;
                        bullet.velocity = direction2Player.normalized * 5;
                        bullet.gameObject.SetActive(true);

                        var soundIndex = Random.Range(0, ShootSounds.Count);
                        AudioKit.PlaySound(ShootSounds[soundIndex]);

                        if (direction2Player.x < 0)
                        {
                            spriteRenderer.flipX = true;
                        }
                        else
                        {
                            spriteRenderer.flipX = false;
                        }
                    }
                })
                .OnUpdate(() =>
                {

                    if (State.SecondsOfCurrentState >= shootScd)
                    {
                        State.ChangeState(States.FollowPlayer);
                    }
                });

            State.StartState(States.FollowPlayer);
        }

        // Update is called once per frame
        void Update() => State.Update();

        public void Hurt(float damage)
        {
            Hp -= damage;
            if (Hp <= 0f)
            {
                Destroy(gameObject);
            }
        }

    }
}
