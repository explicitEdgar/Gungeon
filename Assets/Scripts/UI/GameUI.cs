using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QFramework.Gungeon
{
    public partial class GameUI : ViewController
    {
        public static GameUI Default;

        public GameObject gamePass;

        public GameObject gameOver;

        public Text HPText;

        private void Awake()
        {
            Default = this;
        }

        private void OnDestroy()
        {
            Default = null;

        }
        // Start is called before the first frame update
        void Start()
        {
            gamePass.transform.Find("RestartBtn").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("SampleScene");
                    Global.ResetData();
                });

            gameOver.transform.Find("RestartBtn").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    SceneManager.LoadScene("SampleScene");
                    Global.ResetData();
                });

            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinInfo.text = coin.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.Key.RegisterWithInitValue(key =>
            {
                KeyInfo.text = key.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.HP.Or(Global.MaxHP).Or(Global.Armor).Register(() =>
            {
                UpdateHPAndArmorView();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            UpdateHPAndArmorView();
        }

        void UpdateHPAndArmorView()
        {
            HpArmorBg.DestroyChildrenWithCondition(item => item != HP.transform && item != Armor.transform);
            //用下面这种会导致子模版被删除而无法正常clone
            //HpArmorBg.DestroyChildren();

            for(int i = 0;i < Global.MaxHP.Value / 2;i++)
            {
                var hp = HP.InstantiateWithParent(HpArmorBg)
                    .Show();

                var result = Global.HP.Value - i * 2;
                var image = hp.transform.Find("Value").GetComponent<Image>();

                if(result > 0)
                {
                    if(result == 1)
                    {
                        image.fillAmount = 0.5f;
                    }
                    else
                    {
                        image.fillAmount = 1;
                    }
                }
                else
                {
                    image.fillAmount = 0;
                }
            }

            for(int i = 0;i < Global.Armor.Value;i++)
            {
                Armor.InstantiateWithParent(HpArmorBg)
                    .Show();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.M))
            {
                if(UImap.gameObject.activeSelf)
                {
                    UImap.Hide();
                }
                else
                {
                    UImap.Show();
                }
            }
        }

        public static void PlayerHurtFlashScreen()
        {
            ActionKit.Sequence()
                .Lerp01(0.01f,p =>
                {
                    Default.ScreenColor.ColorAlpha(p);
                },() =>
                {
                    Default.ScreenColor.ColorAlpha(1);
                })
                .Lerp(1, 0, 0.3f, p =>
                {
                    Default.ScreenColor.ColorAlpha(p);
                }, () =>
                {
                    Default.ScreenColor.ColorAlpha(0);
                })
                .StartCurrentScene()
                .IgnoreTimeScale();
        }

        public static void UpdateGunInfo(Clip clip)
        {
            var bulletBag = Player.Default.gun.bulletBag;
            if (bulletBag.MaxBulletCount == -1)
            {
                GameUI.Default.GunInfo.text = "弹夹容量:" + clip.Data.CurrentBulletCount + "/" + clip.Data.Config.ClipBulletCount + "(\u211e)" + "(按R换弹)";
            }
            else
            {
                GameUI.Default.GunInfo.text = "弹夹容量:" + clip.Data.CurrentBulletCount + "/" + clip.Data.Config.ClipBulletCount + "(" + bulletBag.Data.GunBagRemainBulletCount + "/" + bulletBag.MaxBulletCount + ")" + "(按R换弹)";
            }
        }

    }
}
