using QFramework.Gungeon;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Search;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class Global
    {
        public static Player player;

        public static Room currentRoom;

        public static List<LevelConfig> Levels = new List<LevelConfig>()
        {
            Level1.Config,
            Level2.Config,
        };

        public static LevelConfig CurrentLevel;

        public static GunData CurrentGun;

        public static BindableProperty<int> HP = new BindableProperty<int>(3);

        public static BindableProperty<int> MaxHP = new BindableProperty<int>(6);

        public static BindableProperty<int> Armor = new BindableProperty<int>(1);

        public static BindableProperty<int> Coin = new BindableProperty<int>();

        public static BindableProperty<int> Key = new BindableProperty<int>();

        public static DynaGrid<Room> RoomGrid { get; set; }

        public static bool UIOpened = false;

        public static bool CanDo => !UIOpened;

        public static Queue<int> CurrentPacing = null;

        public static float GunAddtionSize;

        public static Vector2 CameraOffset { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoInit()
        {
            ResetData();
        }

        public static void ResetData()
        {
            UIOpened = false;
            Coin.Value = 0;
            Key.Value = 0;
            HP.Value = 3;
            MaxHP.Value = 6;
            Armor.Value = 1;
            Time.timeScale = 1;

            GunSystem.GunList.Clear();
            GunSystem.GunList.Add(GunConfig.Pistol.CreateData());
            GunSystem.GunList.Add(GunConfig.MP5.CreateData());
            GunSystem.GunList.Add(GunConfig.AK.CreateData());
            GunSystem.GunList.Add(GunConfig.AWP.CreateData());
            GunSystem.GunList.Add(GunConfig.ShotGun.CreateData());
            GunSystem.GunList.Add(GunConfig.Laser.CreateData());

            CurrentGun = GunSystem.GunList.First();

            CurrentLevel = Level2.Config;
            CurrentPacing = new Queue<int>(CurrentLevel.Pacing);
        }

        public static bool NextLevel()
        {
            var levelIndex = Global.Levels.FindIndex(l => l == Global.CurrentLevel);

            levelIndex++;

            if(levelIndex == Global.Levels.Count)
            {
                //”Œœ∑Õ®πÿ
                return false;
            }
            else
            {
                CurrentLevel = Levels[levelIndex];

                return true;
            }
        }
    }
}
