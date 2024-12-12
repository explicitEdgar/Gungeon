using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    public EnemyBullet enemyBullet;

    public Rigidbody2D mrigidbody2D;

    public enum States
    {
        FollowPlayer,
        Shoot,
    }

    public States state = States.FollowPlayer;

    public float followPlayerScd = 3.0f;

    public float shootScd = 1.0f;

    public float currentScd = 0f;

    public SpriteRenderer spriteRenderer;

    public List<AudioClip> ShootSounds = new List<AudioClip>();

    public AudioSource ShootSoundPlayer;

    private float Hp = 3;


    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == States.FollowPlayer)
        {
            currentScd += Time.deltaTime;

            if (currentScd >= followPlayerScd)
            {
                state = States.Shoot;
                currentScd = 0;
            }

            if (Global.player)
            {
                var direction2Player = (Global.player.transform.position - transform.position).normalized;

                mrigidbody2D.velocity = direction2Player;

                if(direction2Player.x < 0)
                {
                    spriteRenderer.flipX = true;
                }
                else
                {
                    spriteRenderer.flipX = false;
                }
            }
        }
        else if (state == States.Shoot)
        {
            currentScd += Time.deltaTime;

            if(currentScd >= shootScd)
            {
                state = States.FollowPlayer;
                currentScd = 0;

                followPlayerScd = Random.Range(1.0f, 4.0f);
            }

            mrigidbody2D.velocity = new Vector2(0,0);

            if (Time.frameCount % 20 == 0)
            {
                if(Global.player)
                {
                    var bullet = Instantiate(enemyBullet);
                    bullet.transform.position = transform.position;
                    var direction2Player = (Global.player.transform.position - transform.position).normalized;
                    bullet.direction = direction2Player;
                    bullet.gameObject.SetActive(true);

                    var soundIndex = Random.Range(0, ShootSounds.Count);
                    ShootSoundPlayer.clip = ShootSounds[soundIndex];
                    ShootSoundPlayer.Play();

                    if (direction2Player.x < 0)
                    {
                        spriteRenderer.flipX = true;
                    }
                    else
                    {
                        spriteRenderer.flipX = false;
                    }
                }
            }
        }
    }

    public void Hurt(float damage)
    {
        Hp -= damage;
        if(Hp <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
