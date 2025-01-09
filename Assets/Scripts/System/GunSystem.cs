using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
    public class GunSystem
    {
        public static List<GunData> GunList = new List<GunData>()
        {
            GunConfig.Pistol.CreateData(),
            GunConfig.MP5.CreateData(),
            GunConfig.AK.CreateData(),
            GunConfig.AWP.CreateData(),
            GunConfig.Bow.CreateData(),
            GunConfig.Laser.CreateData(),
            GunConfig.RocketGun.CreateData(),
            GunConfig.ShotGun.CreateData(),
        };
    }
}
