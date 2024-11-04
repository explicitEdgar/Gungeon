using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Player player;

    public EnemyBullet enemyBullet;

    public enum States
    {
        FollowPlayer,
        Shoot,
    }

    public States state = States.FollowPlayer;

    public float followPlayerScd = 3.0f;

    public float shootScd = 1.0f;

    public float currentScd = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if(state == States.FollowPlayer)
        {
            currentScd += Time.deltaTime;

            if (currentScd >= followPlayerScd)
            {
                state = States.Shoot;
                currentScd = 0;
            }

            var direction2Player = (player.transform.position - transform.position).normalized;

            transform.Translate(direction2Player * Time.deltaTime);
        }
        else if(state == States.Shoot)
        {
            currentScd += Time.deltaTime;

            if(currentScd >= shootScd)
            {
                state = States.FollowPlayer;
                currentScd = 0;

                followPlayerScd = Random.Range(1.0f, 4.0f);
            }

            if(Time.frameCount % 20 == 0)
            {
                var bullet = Instantiate(enemyBullet);
                bullet.transform.position = transform.position;
                var direction2Player = (player.transform.position - transform.position).normalized;
                bullet.direction = direction2Player;
                bullet.gameObject.SetActive(true);
            }
        }
    }
}
