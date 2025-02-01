using QFramework;
using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyD : Enemy, IEnemy
{
    
    public Player player;

    public EnemyBullet enemyBullet;

    
    public float followPlayerScd = 3.0f;

    public float shootScd = 1.0f;

    

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
                MovementPath.Clear();
            })
            .OnUpdate(() =>
            {
                FollowPlayer();

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
                    Rigidbody2D.velocity = new Vector2(0, 0);

                    BulletHelper.ShootAround(18, transform.Position2D(), 0.5f, enemyBullet);

                    var soundIndex = Random.Range(0, ShootSounds.Count);
                    AudioKit.PlaySound(ShootSounds[soundIndex]);
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

    private void OnDestroy()
    {
        Room.Enemies.Remove(this);
    }

    public override void Hurt(float damage,Vector2 hitDirection)
    {
        FxFactory.Default.GenerateHurtFx(transform.Position2D());
        FxFactory.Default.GenerateEnemyBlood(transform.Position2D());

        Hp -= damage;
        if (Hp <= 0f)
        {
            OnDeath(hitDirection, "EnemyDDie", 1.5f);

        }
    }
}
