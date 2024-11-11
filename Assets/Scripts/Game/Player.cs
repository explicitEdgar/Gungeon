using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerBullet playerBullet;

    public Rigidbody2D mrigidbody2D;

    public SpriteRenderer spriteRenderer;

    public Transform weapon;

    public Pistol pistol;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        //×Óµ¯³¯Ïò
        var mouseScreePosition = Input.mousePosition;
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreePosition);
        var bulletDirection = (mouseWorldPosition - transform.position).normalized;

        //ÎäÆ÷Ðý×ª
        var radius = Mathf.Atan2(bulletDirection.y, bulletDirection.x);
        var eulerAngles = radius * Mathf.Rad2Deg;
        weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);

        //ÎäÆ÷Ðý×ª¾ÀÕý
        if(bulletDirection.x > 0)
        {
            weapon.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            weapon.transform.localScale = new Vector3(1, -1, 1);
        }

        mrigidbody2D.velocity = new Vector3(horizontal, vertical).normalized * 5;

        if(horizontal < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        if(Input.GetMouseButtonDown(0))
        {
            pistol.ShootDown(bulletDirection);
        }
    }

    public void hurt(int damage)
    {
        Global.HP -= damage;
        if(Global.HP <= 0)
        {
            Global.HP = 0;
            GameUI.Default.gameOver.SetActive(true);
            Time.timeScale = 0;
        }
        Global.HPChangedEvent();
    }
}
