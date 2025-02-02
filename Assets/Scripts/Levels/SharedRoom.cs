using System.Collections.Generic;

namespace QFramework.Gungeon
{
    public class SharedRoom
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
            .L("1    y y y y      1")
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
            .L("d        #e       d")
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
          .L("1    s   s   s    1")
          .L("1                 1")
          .L("d    s   s   s    d")
          .L("1                 1")
          .L("1    s   s   s    1")
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