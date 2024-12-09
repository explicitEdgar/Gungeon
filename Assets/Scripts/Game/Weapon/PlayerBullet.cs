using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public Vector2 direction;

    private Rigidbody2D mRigidbody2D;
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
        mRigidbody2D.velocity = direction * 24;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.GetComponent<Enemy>())
        {
            other.gameObject.SetActive(false);
        }
        Destroy(gameObject);
    }
}
