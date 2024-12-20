using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Enemy>())
        {
            other.gameObject.GetComponent<Enemy>().Hurt(Damage);
        }
        Destroy(gameObject);
    }
}
