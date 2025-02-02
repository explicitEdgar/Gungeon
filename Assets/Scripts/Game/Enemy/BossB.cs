using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{

    public class BossB : Enemy, IEnemy
    {
        
        public Player player;

        public EnemyBullet enemyBullet;

        
        public float followPlayerScd = 3.0f;

        public float shootScd = 1.0f;

        

        public List<AudioClip> ShootSounds = new List<AudioClip>();

        private float Hp = 250;

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

            State.State(States.Shoot)
                .OnEnter(() =>
                {
                    Rigidbody2D.velocity = new Vector2(0, 0);

                    //Ò»½×¶Î
                    if (Hp / maxHp > 0.7)
                    {
                        if (Global.player)
                        {
                            Rigidbody2D.velocity = new Vector2(0, 0);

                            BulletHelper.ShootAround(10, transform.Position2D(), 0.5f, enemyBullet);

                            var soundIndex = Random.Range(0, ShootSounds.Count);
                            AudioKit.PlaySound(ShootSounds[soundIndex]);
                        }
                    }
                    //¶þ½×¶Î
                    else if (Hp / maxHp > 0.3)
                    {
                        if (Global.player)
                        {   
                            Rigidbody2D.velocity = new Vector2(0, 0);

                            ActionKit.Sequence()
                            .Callback(() =>
                            {
                                BulletHelper.ShootAround(15, transform.Position2D(), 0.5f, enemyBullet);
                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Delay(0.2f)
                            .Callback(() =>
                            {
                                BulletHelper.ShootAround(15, transform.Position2D(), 0.5f, enemyBullet);
                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Delay(0.2f)
                            .Callback(() =>
                            {
                                BulletHelper.ShootAround(15, transform.Position2D(), 0.5f, enemyBullet);
                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
                            })
                            .Start(this);
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
                        if (State.FrameCountOfCurrentState % 15 == 0)
                        {
                            if (Global.player)
                            {
                                Rigidbody2D.velocity = new Vector2(0, 0);

                                BulletHelper.ShootAround(18, transform.Position2D(), 0.5f, enemyBullet);

                                var soundIndex = Random.Range(0, ShootSounds.Count);
                                AudioKit.PlaySound(ShootSounds[soundIndex]);
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
            FxFactory.Default.GenerateHurtFx(transform.Position2D());
            FxFactory.Default.GenerateEnemyBlood(transform.Position2D());

            Hp -= damage;
            GameUI.Default.BossHp.fillAmount = Hp / maxHp;
            if (Hp <= 0f)
            {
                OnDeath(hitDirection, "EnemyHDie", 1.5f);
            }
        }

    }
}
