using QFramework.Gungeon;
using System;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class Global
    {
        public static Player player;

        public static Room currentRoom;

        public static BindableProperty<int> HP = new BindableProperty<int>(3);

        public static BindableProperty<int> Armor = new BindableProperty<int>(1);

        public static BindableProperty<int> Coin = new BindableProperty<int>();

        public static DynaGrid<Room> RoomGrid { get; set; }

        public static bool UIOpened = false;

        public static bool CanDo => !UIOpened;

        public static void ResetData()
        {
            UIOpened = false;
            Coin.Value = 0;
            HP.Value = 3;
            Armor.Value = 1;
            Time.timeScale = 1;
        }
    }
}
