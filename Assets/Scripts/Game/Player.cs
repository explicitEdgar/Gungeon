using QFramework.Gungeon;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

namespace QFramework.Gungeon
{
    public partial class Player : ViewController
    {   
        public enum States
        {
            Idle,
            Rolling
        }

        public FSM<States> State = new FSM<States>();

        public static void DisplayText(string text,float duration = 1.0f)
        {
            Default.StartCoroutine(Default.DoDisplayText(text, duration));
        }

        IEnumerator DoDisplayText(string text,float duration)
        {
            Text.text = text;
            Text.Show();
            yield return new WaitForSeconds(duration);
            Text.Hide();
        }

        public PlayerBullet playerBullet;

        public Rigidbody2D mrigidbody2D;

        public SpriteRenderer spriteRenderer;

        public Transform weapon;

        public Gun gun;

        public List<AudioClip> gunTakeSfxs = new List<AudioClip>();

        public static Player Default;

        private void Awake()
        {   
            Default = this;

            Application.targetFrameRate = 60;

            AudioKit.PlaySoundMode = AudioKit.PlaySoundModes.IgnoreSameSoundInSoundFrames;

            Text.Hide();

            gunTakeSfxs.Add(GunTake1);
            gunTakeSfxs.Add(GunTake2);
            gunTakeSfxs.Add(GunTake3);
            gunTakeSfxs.Add(GunTake4);
            gunTakeSfxs.Add(GunTake5);

            UseGun(0);
        }

        public Gun GunwithKey(string key)
        {   
            if(key == GunConfig.Pistol.Key)
                return Pistol;
            if(key == GunConfig.MP5.Key)
                return MP5;
            if (key == GunConfig.AK.Key)
                return AK;
            if (key == GunConfig.AWP.Key)
                return AWP;
            if (key == GunConfig.Bow.Key)
                return Bow;
            if (key == GunConfig.Laser.Key)
                return Laser;
            if (key == GunConfig.RocketGun.Key)
                return RocketGun;
            if (key == GunConfig.ShotGun.Key)
                return ShotGun;

            return null;
        }
        public void UseGun(int index)
        {
            var gundata = GunSystem.GunList[index];
            gun.Hide();
            gun = GunwithKey(gundata.Key);
            gun.WithData(gundata);
            Global.CurrentGun = gundata;
            gun.Show();
            gun.OnGunUse();

            var gunTakeSfx = gunTakeSfxs.GetRandomItem();
            SelfAudioSource.clip = gunTakeSfx;
            SelfAudioSource.Play();

            Global.GunAddtionSize = gun.GunAddtionSize;
        }

        // Start is called before the first frame update
        void Start()
        {
            var gunIndex = GunSystem.GunList.FindIndex(g => g == Global.CurrentGun);
            UseGun(gunIndex);

            State.State(States.Idle)
                .OnUpdate(() =>
                {
                    var horizontal = Input.GetAxisRaw("Horizontal");
                    var vertical = Input.GetAxisRaw("Vertical");

                    //子弹朝向
                    var mouseScreePosition = Input.mousePosition;
                    var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreePosition);
                    var bulletDirection = (mouseWorldPosition - transform.position).normalized;

                    //自动瞄准
                    if (Global.currentRoom && Global.currentRoom.Enemies.Count > 0)
                    {
                        targetEnemy = Global.currentRoom.Enemies
                        .OrderBy(e => (e.GameObject.Position2D() - mouseWorldPosition.ToVector2()).magnitude)
                        .FirstOrDefault(e =>
                        {
                            var direction = this.Direction2DTo(e.GameObject);

                            //有墙挡着
                            if (Physics2D.Raycast(this.Position2D(), direction.normalized, direction.magnitude, LayerMask.GetMask("Wall")))
                            {
                                return false;
                            }

                            return true;
                        });

                        if (targetEnemy != null && targetEnemy.GameObject)
                        {
                            bulletDirection = this.NormalizedDirection2DTo(targetEnemy.GameObject);
                            Aim.Position2D(targetEnemy.GameObject.Position2D());
                            Aim.Show();
                        }
                        else
                        {
                            Aim.Hide();
                        }
                    }
                    else
                    {
                        Aim.Hide();
                    }

                    //武器旋转
                    var radius = Mathf.Atan2(bulletDirection.y, bulletDirection.x);
                    var eulerAngles = radius * Mathf.Rad2Deg;
                    weapon.localRotation = Quaternion.Euler(0, 0, eulerAngles);

                    //武器旋转纠正
                    if (bulletDirection.x > 0)
                    {
                        weapon.transform.localScale = new Vector3(1, 1, 1);
                        spriteRenderer.flipX = false;
                    }
                    else if (bulletDirection.x < 0)
                    {
                        weapon.transform.localScale = new Vector3(1, -1, 1);
                        spriteRenderer.flipX = true;
                    }

                    mrigidbody2D.velocity = new Vector3(horizontal, vertical).normalized * 5;

                    if (horizontal != 0 || vertical != 0)
                    {
                        AnimationHelper.UpDownAnimation(Sprite, 0.05f, Time.frameCount, 10);
                        AnimationHelper.UpDownAnimation(weapon, 0.05f, Time.frameCount, 10);
                        AnimationHelper.RotateAnimation(Sprite, 5, Time.frameCount, 30);
                    }

                    var offsetLength = (mouseWorldPosition - transform.position).magnitude;
                    Global.CameraOffset = (bulletDirection * (3 + Mathf.Clamp(offsetLength * 0.15f, 0, 3))).ToVector2();

                    if (Global.CanDo)
                    {
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
                        if (Input.GetKeyDown(KeyCode.R))
                        {
                            gun.Reload();
                        }
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            var index = GunSystem.GunList.FindIndex(gun1 => gun1 == gun.Data);
                            index++;
                            if (index > GunSystem.GunList.Count - 1)
                            {
                                index = 0;
                            }

                            UseGun(index);
                        }
                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            var index = GunSystem.GunList.FindIndex(gun1 => gun1 == gun.Data);
                            index--;
                            if (index < 0)
                            {
                                index = GunSystem.GunList.Count - 1;
                            }

                            UseGun(index);
                        }
                        if(Input.GetMouseButtonDown(1))
                        {
                            if(horizontal != 0 || vertical != 0)
                            {
                                State.ChangeState(States.Rolling);
                            }
                        }
                    }
                });

            var faceDirection = Vector2.zero;
            State.State(States.Rolling)
                .OnEnter(() =>
                {
                    SelfCircleCollider2D.Disable();
                    Aim.Hide();

                    var x = Input.GetAxis("Horizontal");
                    var y = Input.GetAxis("Vertical");

                    if(x != 0 || y != 0)
                    {
                        faceDirection = new Vector2(x, y).normalized;
                    }

                    ActionKit.Lerp(0, 1, 0.4f, p =>
                    {
                        p = EaseUtility.InCubic(0, 1, p);

                        if(x > 0)
                        {
                            Sprite.LocalEulerAnglesZ(p * -360f);
                        }
                        else
                        {
                            Sprite.LocalEulerAnglesZ(p * 360f);
                        }
                    },
                    () =>
                    {
                        Sprite.LocalEulerAnglesZ(0);
                        State.ChangeState(States.Idle);
                    }).Start(this);
                })
                .OnFixedUpdate(() =>
                {
                    SelfRigidbody2D.velocity = faceDirection * 8;
                })
                .OnExit(() =>
                {
                    SelfCircleCollider2D.Enable();
                });
            State.StartState(States.Idle);
        }

        private void OnDestroy()
        {
            Default.DestroySelf();
        }

        private IEnemy targetEnemy = null;
        // Update is called once per frame
        void Update()
        {
            State.Update();
        }

        private void FixedUpdate()
        {
            State.FixedUpdate();
        }

        public void hurt(int damage)
        {   
            if(Global.Armor.Value > 0)
            {
                if(Global.Armor.Value >= damage)
                {
                    Global.Armor.Value -= damage;
                    damage = 0;
                    AudioKit.PlaySound("Resources://UseArmor");
                    return;
                }
                else
                {
                    damage -= Global.Armor.Value;
                    Global.Armor.Value = 0;
                    AudioKit.PlaySound("Resources://UseArmor");
                }
            }


            FxFactory.Default.GenerateHurtFx(transform.Position2D(),Color.green);

            AudioKit.PlaySound("Resources://PlayerHurt");

            FxFactory.Default.GeneratePlayerBlood(transform.Position2D());

            Global.HP.Value -= damage;
            if (Global.HP.Value <= 0)
            {
                AudioKit.PlaySound("Resources://PlayerDie");
                Global.HP.Value = 0;
                GameUI.Default.gameOver.SetActive(true);
                Global.UIOpened = true;
                Time.timeScale = 0;
            }

            GameUI.PlayerHurtFlashScreen();
        }

    }
}
