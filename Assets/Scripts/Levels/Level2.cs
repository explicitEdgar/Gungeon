using System;
using System.Collections.Generic;
using UnityEngine;
namespace QFramework.Gungeon
{
    public class Level2
    {
        public static LevelConfig Config = new LevelConfig()
            .NormalRooms(Lv2Rooms.normalRooms)
            .AddBoss(Constant.BossB)
            .Self(self =>
            {
                self.Pacing = new List<int>()
                {
                    2,
                    2,
                    3,
                    2,
                    3,
                    2,
                    3,
                    3,
                    1,
                    2,
                };
            })
            .Self(self =>
            {
                var randomIndex = UnityEngine.Random.Range(0, 0 + 1);

                switch(randomIndex)
                {
                    case 0:
                        self.InitRoom
                            .Branch(node =>
                            {
                                node.Next(RoomTypes.Normal);
                            })
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Shop, node =>
                            {
                                node.Next(RoomTypes.Normal)
                                    .Next(RoomTypes.Normal)
                                    .Next(RoomTypes.Chest);
                            })
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Final);
                        break;
                    default:
                        break;
                }
                



            });
    }
}