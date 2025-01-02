using QFramework.Gungeon;
using System;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class Global
    {
        public static Player player;

        public static Room currentRoom;

        public static int HP = 100;

        public static Action HPChangedEvent;

        public static BindableProperty<int> Coin = new BindableProperty<int>();

        public static DynaGrid<Room> RoomGrid { get; set; }

        public static bool UIOpened = false;

        public static bool CanDo => !UIOpened;

        public static void ResetData()
        {
            UIOpened = false;
            Coin.Value = 0;
            HP = 3;
            Time.timeScale = 1;
        }
    }
}
