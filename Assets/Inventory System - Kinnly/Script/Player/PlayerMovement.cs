using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Kinnly
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Core")]
        [SerializeField] float speed;

        [HideInInspector] public Vector2 direction;
        [HideInInspector] public bool isUsingTools;

        Rigidbody2D rb;
        Animator animator;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
        }

        void Movement()
        {
            if (isUsingTools)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            float xMovement = Input.GetAxisRaw("Horizontal");
            float yMovement = Input.GetAxisRaw("Vertical");

            Vector2 movement = new Vector2(xMovement, yMovement).normalized * speed;
            rb.velocity = movement;

            if (xMovement != 0f || yMovement != 0f)
            {
                direction = new Vector2(xMovement, yMovement);
                animator.SetFloat("Horizontal", xMovement);
                animator.SetFloat("Vertical", yMovement);
                animator.SetBool("Run", true);
            }
            else
            {
                animator.SetBool("Run", false);
            }
        }

        public void SetDirection(Vector2 _direction)
        {
            direction.x = Mathf.Round(_direction.x);
            direction.y = Mathf.Round(_direction.y);
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
        }
    }
}