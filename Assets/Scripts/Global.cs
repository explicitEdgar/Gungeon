using QFramework.Gungeon;
using System;
using UnityEngine;

public class Global
{
    public static Player player;

    public static int HP = 3;

    public static Action HPChangedEvent;

    public static void ResetData()
    {
        HP = 3;
        Time.timeScale = 1;
    }
}
