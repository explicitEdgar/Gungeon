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

            Global.HPChangedEvent -= UpdateHPText;
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

            Global.HPChangedEvent += UpdateHPText;

            Global.Coin.RegisterWithInitValue(coin =>
            {
                CoinInfo.text = coin.ToString();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateHPText()
        {
            HPText.text = "ÉúÃüÖµ:" + Global.HP;
        }

        public static void UpdateGunInfo(Clip clip)
        {
            var bulletBag = Player.Default.gun.bulletBag;
            if (bulletBag.MaxBulletCount == -1)
            {
                GameUI.Default.GunInfo.text = "µ¯¼ÐÈÝÁ¿:" + clip.currentClipBullet + "/" + clip.clipBullet + "(\u211e)" + "(°´R»»µ¯)";
            }
            else
            {
                GameUI.Default.GunInfo.text = "µ¯¼ÐÈÝÁ¿:" + clip.currentClipBullet + "/" + clip.clipBullet + "(" + bulletBag.RemainBulletCount + "/" + bulletBag.MaxBulletCount + ")" + "(°´R»»µ¯)";
            }
        }

    }
}
