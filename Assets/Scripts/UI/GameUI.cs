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

            Global.HP.RegisterWithInitValue(hp =>
            {
                HPText.text = "ÉúÃüÖµ£º" + hp.ToString();
            });

            Global.Armor.RegisterWithInitValue(armor =>
            {
                ArmorText.text = "»¤¶Ü£º" + armor.ToString();
            });

            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinInfo.text = coin.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
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

        public static void UpdateGunInfo(Clip clip)
        {
            var bulletBag = Player.Default.gun.bulletBag;
            if (bulletBag.MaxBulletCount == -1)
            {
                GameUI.Default.GunInfo.text = "µ¯¼ÐÈÝÁ¿:" + clip.Data.CurrentBulletCount + "/" + clip.Data.Config.ClipBulletCount + "(\u211e)" + "(°´R»»µ¯)";
            }
            else
            {
                GameUI.Default.GunInfo.text = "µ¯¼ÐÈÝÁ¿:" + clip.Data.CurrentBulletCount + "/" + clip.Data.Config.ClipBulletCount + "(" + bulletBag.Data.GunBagRemainBulletCount + "/" + bulletBag.MaxBulletCount + ")" + "(°´R»»µ¯)";
            }
        }

    }
}
