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

        public static void ResetData()
        {
            HP = 3;
            Time.timeScale = 1;
        }
    }
}
