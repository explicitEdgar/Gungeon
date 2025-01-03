using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{

    public enum RoomTypes
    {
        Init,
        Normal,
        Chest,
        Shop,
        Final,
    }

    public class EnemyWaveConfig
    {
        //敌人波次配置代码
    }
    
    //配置类
    public class Config
    {
        /*
            1代表地块
            @代表玩家
            e代表敌人
            #代表出口
            d代表门
            c代表宝箱
            s代表商店摊位
        */
        public static RoomConfig initRoom = new RoomConfig()
            .Type(RoomTypes.Init)
            .L("111111111d111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("d        @        d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("111111111d111111111");

        public static List<RoomConfig> normalRooms = new List<RoomConfig>()
        {
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("111111111d111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1        e        1")
            .L("d       e1e       d")
            .L("1        e        1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("111111111d111111111"),
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("111111111d111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   1e      e1    1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("d        1        d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1   1e      e1    1")
            .L("1                 1")
            .L("1                 1")
            .L("111111111d111111111"),
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("111111111d111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   1111  1111    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1       eee       1")
            .L("d       eee       d")
            .L("1       eee       1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1111  1111    1")
            .L("1                 1")
            .L("1                 1")
            .L("111111111d111111111"),
    };

        public static RoomConfig chestRoom = new RoomConfig()
           .Type(RoomTypes.Chest)
           .L("111111111d111111111")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("d        c        d")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("1                 1")
           .L("111111111d111111111");

        public static RoomConfig finalRoom = new RoomConfig()
            .Type(RoomTypes.Final)
            .L("111111111d111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("d        #        d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("111111111d111111111");

        public static RoomConfig shopRoom = new RoomConfig()
          .Type(RoomTypes.Shop)
          .L("111111111d111111111")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("d        s        d")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("1                 1")
          .L("111111111d111111111");
    }

}