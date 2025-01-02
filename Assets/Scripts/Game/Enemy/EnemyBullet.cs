using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public Vector2 velocity;

    public Rigidbody2D mrigidbody2D;
    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {
        mrigidbody2D.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            other.gameObject.GetComponent<Player>().hurt(1);
        }
        Destroy(gameObject);
    }
}
