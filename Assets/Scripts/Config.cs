using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{

    public enum RoomTypes
    {
        Init,
        Normal,
        Final,
    }

    public class EnemyWaveConfig
    {
        //敌人波次配置代码
    }
    
    //配置房间
    public class Config
    {
        /*
            1代表地块
            @代表玩家
            e代表敌人
            #代表出口
            d代表门
        */
        public static RoomConfig initRoom = new RoomConfig()
            .Type(RoomTypes.Init)
            .L("1111111111111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 d")
            .L("1        @        d")
            .L("1                 d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1111111111111111111");

        public static List<RoomConfig> normalRooms = new List<RoomConfig>()
        {
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("1111111111111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("d        e        d")
            .L("d       e1e       d")
            .L("d        e        d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("1111111111111111111"),
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("1111111111111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   1e      e1    1")
            .L("1   e        e    1")
            .L("1                 1")
            .L("1                 1")
            .L("d                 d")
            .L("d        1        d")
            .L("d                 d")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1   e        e    1")
            .L("1   1e      e1    1")
            .L("1                 1")
            .L("1                 1")
            .L("1111111111111111111"),
            new RoomConfig()
            .Type(RoomTypes.Normal)
            .L("1111111111111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1   1111  1111    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("d       eee       d")
            .L("d       eee       d")
            .L("d       eee       d")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1        1    1")
            .L("1   1111  1111    1")
            .L("1                 1")
            .L("1                 1")
            .L("1111111111111111111"),
    };    
        
        public static RoomConfig finalRoom = new RoomConfig()
            .Type(RoomTypes.Final)
            .L("1111111111111111111")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("d                 1")
            .L("d              #  1")
            .L("d                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1                 1")
            .L("1111111111111111111");
    }

}