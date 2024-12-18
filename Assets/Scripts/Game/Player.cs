using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework.Gungeon
{
    public partial class Player : ViewController
    {
        public PlayerBullet playerBullet;

        public Rigidbody2D mrigidbody2D;

        public SpriteRenderer spriteRenderer;

        public Transform weapon;

        public Gun gun;

        public List<Gun> Gunlist = new List<Gun>();

        public List<AudioClip> gunTakeSfxs = new List<AudioClip>();

        public static Player Default;

        private void Awake()
        {
            Default = this;

            Gunlist.Add(AK);
            Gunlist.Add(AWP);
            Gunlist.Add(Bow);
            Gunlist.Add(Laser);
            Gunlist.Add(MP5);
            Gunlist.Add(Pistol);
            Gunlist.Add(RocketGun);
            Gunlist.Add(ShotGun);

            gunTakeSfxs.Add(GunTake1);
            gunTakeSfxs.Add(GunTake2);
            gunTakeSfxs.Add(GunTake3);
            gunTakeSfxs.Add(GunTake4);
            gunTakeSfxs.Add(GunTake5);

            UseGun(0);
        }

        private void UseGun(int index)
        {
            gun.Hide();
            gun = Gunlist[index];
            gun.Show();
            gun.OnGunUse();

            var gunTakeSfx = gunTakeSfxs.GetRandomItem();
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        private void OnDestroy()
        {
            Default.DestroySelf();
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
            if (bulletDirection.x > 0)
            {
                weapon.transform.localScale = new Vector3(1, 1, 1);
                spriteRenderer.flipX = false;
            }
            else if(bulletDirection.x < 0)
            {
                weapon.transform.localScale = new Vector3(1, -1, 1);
                spriteRenderer.flipX = true;
            }

            mrigidbody2D.velocity = new Vector3(horizontal, vertical).normalized * 5;

            if (Input.GetMouseButtonDown(0))
            {
                gun.ShootDown(bulletDirection);
            }
            if (Input.GetMouseButton(0))
            {
                gun.Shooting(bulletDirection);
            }
            if (Input.GetMouseButtonUp(0))
            {
                gun.ShootUp(bulletDirection);
            }
            if (Input.GetKey(KeyCode.R))
            {
                gun.Reload();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                var index = Gunlist.FindIndex(gun1 => gun1 == gun);
                index++;
                if(index > Gunlist.Count - 1)
                {
                    index = 0;
                }

                UseGun(index);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                var index = Gunlist.FindIndex(gun1 => gun1 == gun);
                index--;
                if (index < 0)
                {
                    index = Gunlist.Count - 1;
                }

                UseGun(index);
            }
        }

        public void hurt(int damage)
        {
            Global.HP -= damage;
            if (Global.HP <= 0)
            {
                Global.HP = 0;
                GameUI.Default.gameOver.SetActive(true);
                Time.timeScale = 0;
            }
            Global.HPChangedEvent();
        }

    }
}
