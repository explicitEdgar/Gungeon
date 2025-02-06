using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static QFramework.Gungeon.UIGunList;

namespace QFramework.Gungeon
{   
    public class GunConfig
    {
        public string Key;
        public int ClipBulletCount;
        public int GunBagMaxBulletCount;
        public static GunConfig Pistol = new GunConfig()
        {
            Key = "pistol",
            ClipBulletCount = 10,
            GunBagMaxBulletCount = -1,
        };
        public static GunConfig MP5 = new GunConfig()
        {
            Key = "MP5",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 60,
        };
        public static GunConfig AK = new GunConfig()
        {
            Key = "AK",
            ClipBulletCount = 30,
            GunBagMaxBulletCount = 90,
        };
        public static GunConfig AWP = new GunConfig()
        {
            Key = "AWP",
            ClipBulletCount = 10,
            GunBagMaxBulletCount = 20,
        };
        public static GunConfig Bow = new GunConfig()
        {
            Key = "Bow",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 30,
        };
        public static GunConfig Laser = new GunConfig()
        {
            Key = "Laser",
            ClipBulletCount = 1000,
            GunBagMaxBulletCount = 2000,
        };
        public static GunConfig RocketGun = new GunConfig()
        {
            Key = "RocketGun",
            ClipBulletCount = 5,
            GunBagMaxBulletCount = 10,
        };
        public static GunConfig ShotGun = new GunConfig()
        {
            Key = "ShotGun",
            ClipBulletCount = 8,
            GunBagMaxBulletCount = 16,
        };

        public static List<GunConfig> Configs = new List<GunConfig>()
        {
            GunConfig.ShotGun,
            GunConfig.MP5,
            GunConfig.AK,
            GunConfig.Bow,
            GunConfig.RocketGun,
            GunConfig.Laser,
            GunConfig.AWP
        };

        public GunData CreateData()
        {
            return new GunData()
            {
                Key = Key,
                Config = this,
                CurrentBulletCount = this.ClipBulletCount,
                GunBagRemainBulletCount = this.GunBagMaxBulletCount,
            };
        }
    }

    public class GunData
    {
        public string Key;
        public int CurrentBulletCount;
        public int GunBagRemainBulletCount;
        public GunConfig Config;
        public bool Reloading;
    }
    public class GunSystem : AbstractSystem
    {
        public static List<GunData> GunList = new List<GunData>();
        //{
        //    GunConfig.Pistol.CreateData(),
        //    GunConfig.MP5.CreateData(),
        //    GunConfig.AK.CreateData(),
        //    GunConfig.AWP.CreateData(),
        //    GunConfig.Bow.CreateData(),
        //    GunConfig.Laser.CreateData(),
        //    GunConfig.RocketGun.CreateData(),
        //    GunConfig.ShotGun.CreateData(),
        //};

        public class GunBaseItem
        {
            public string Name;
            public string Key;
            public int Price;
            public bool Unlocked = false;
            public Sprite Icon => Player.Default.GunwithKey(Key).Sprite.sprite;
            public bool InitUnlockState;
        }

        public List<GunBaseItem> GunBaseItems => mGunBaseItems;

        void Load()
        {
            foreach (var gunBaseItem in mGunBaseItems)
            {
                gunBaseItem.Unlocked = PlayerPrefs.GetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.InitUnlockState ? 1 : 0) == 1;
            }
        }

        public void Save()
        {
            foreach (var gunBaseItem in mGunBaseItems)
            {
                PlayerPrefs.SetInt(gunBaseItem.Key + "_unlocked", gunBaseItem.Unlocked ? 1 : 0);
            }
        }

        static List<GunBaseItem> mGunBaseItems = new List<GunBaseItem>()
        {
            new() {Name = "霰弹枪",Price = 0,Key = GunConfig.ShotGun.Key,InitUnlockState = true },
            new() {Name = "MP5",Price = 0,Key = GunConfig.MP5.Key,InitUnlockState = true },
            new() {Name = "AK47",Price = 5,Key = GunConfig.AK.Key,InitUnlockState = false },
            new() {Name = "AWP",Price = 6,Key = GunConfig.AWP.Key,InitUnlockState = false },
            new() {Name = "激光枪",Price = 7,Key = GunConfig.Laser.Key,InitUnlockState = false },
            new() {Name = "弓箭",Price = 10,Key = GunConfig.Bow.Key,InitUnlockState = false },
            new() {Name = "火箭筒",Price = 15,Key = GunConfig.RocketGun.Key,InitUnlockState = false },
        };

        public static List<GunConfig> GetAvailableGuns()
        {
            var availableGunConfigs = new List<GunConfig>();

            foreach (var gunConfig in GunConfig.Configs)
            {
                var gunBaseItem = mGunBaseItems.First(gun => gun.Key == gunConfig.Key);
                //如果已解锁且手里没有
                if(gunBaseItem.Unlocked && GunList.All(g => g.Key != gunConfig.Key))
                {
                    availableGunConfigs.Add(gunConfig);
                }
            }

            return availableGunConfigs;
        }

        protected override void OnInit()
        {
            Load();
        }
    }
}
