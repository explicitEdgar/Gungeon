using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{

    public class BossA : Enemy, IEnemy
    {
        
        public Player player;

        public EnemyBullet enemyBullet;

        
        public float followPlayerScd = 3.0f;

        public float shootScd = 1.0f;

        

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        public override bool isBoss => true;

        private float Hp = 200;

        private float maxHp { get; set; }


        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = 60;
            GameUI.Default.BossHpBar.Show();
            GameUI.Default.BossHp.fillAmount = 1.0f;
        }

        public void Awake()
        {
            maxHp = Hp;
            State.State(States.FollowPlayer)
                .OnEnter(() =>
                {
                    if (Hp / maxHp > 0.7)
                    {
                        followPlayerScd = Random.Range(1.0f, 4.0f);
                    }
                    else if(Hp / maxHp > 0.3)
                    {
                        followPlayerScd = Random.Range(1.0f, 2.0f);
                    }
                    else
                    {
                        followPlayerScd = Random.Range(0.5f, 1f);
                    }
                    MovementPath.Clear();
                })
                .OnUpdate(() =>
                {
                    FollowPlayer(2f);

                    if (State.SecondsOfCurrentState >= followPlayerScd)
                    {
                        State.ChangeState(States.Shoot);
                    }
                });

            var count = 0;
            State.State(States.Shoot)
                .OnEnter(() =>
                {
                    Rigidbody2D.velocity = new Vector2(0, 0);

                    count = 0;

                    //Ò»½×¶Î
                    if (Hp / maxHp > 0.7)
                    {
                        if (Global.player)
                        {
                            shootScd = Random.Range(1, 2f);
                            Rigidbody2D.velocity = new Vector2(0, 0);
                            var bullet = Instantiate(enemyBullet);
                            bullet.transform.position = transform.position;
                            var direction2Player = (Global.player.transform.position - transform.position).normalized;
                            bullet.velocity = direction2Player.normalized * 10;
                            bullet.gameObject.SetActive(true);

                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);

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
                    //¶þ½×¶Î
                    else if (Hp / maxHp > 0.3)
                    {
                        if (Global.player)
                        {
                            shootScd = Random.Range(1, 2f);
                            Rigidbody2D.velocity = new Vector2(0, 0);

                            var direction2Player = (Global.player.transform.position - transform.position).normalized;
                            BulletHelper.ShootSpread(3, transform.Position2D(), direction2Player.ToVector2(), 0.5f, enemyBullet, 15f, 10f);

                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);

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
                    //Èý½×¶Î
                    else
                    {
                        shootScd = Random.Range(4, 6f);
                    }
                })
                .OnUpdate(() =>
                {   
                    //Ò»¶þ½×¶Î
                    if (Hp / maxHp > 0.7 || Hp / maxHp > 0.3)
                    {
                        if (State.SecondsOfCurrentState >= shootScd)
                        {
                            State.ChangeState(States.FollowPlayer);
                        }
                    }
                    //Èý½×¶Î
                    else if (Hp / maxHp <= 0.3)
                    {
                        if (State.FrameCountOfCurrentState % 20 == 0)
                        {
                            if (Global.player)
                            {
                                if (count <= 8) count++;
                                var direction2Player = (Global.player.transform.position - transform.position).normalized;
                                BulletHelper.ShootSpread(count, transform.Position2D(), direction2Player.ToVector2(), 0.5f, enemyBullet, 15f);

                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);

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

                        if (State.SecondsOfCurrentState >= shootScd)
                        {
                            State.ChangeState(States.FollowPlayer);
                        }
                    }
                });

            State.StartState(States.FollowPlayer);
        }

        // Update is called once per frame
        void Update() => State.Update();

        private void OnDestroy()
        {
            Room.Enemies.Remove(this);
            GameUI.Default.BossHpBar.Hide();
        }

        public override void Hurt(float damage,Vector2 hitDirection)
        {
            if (isDead) return;
            FxFactory.Default.GenerateHurtFx(transform.Position2D());
            FxFactory.Default.GenerateEnemyBlood(transform.Position2D());

            Hp -= damage;
            GameUI.Default.BossHp.fillAmount = Hp / maxHp;
            if (Hp <= 0f)
            {
                OnDeath(hitDirection, null, 1.5f);
            }
        }

    }
}
