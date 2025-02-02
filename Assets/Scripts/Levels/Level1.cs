using System;
using System.Collections.Generic;
using UnityEngine;
namespace QFramework.Gungeon
{
    public class Level1
    {
        public static LevelConfig Config = new LevelConfig()
            .NormalRooms(Lv1Rooms.normalRooms)
            .Self(self =>
            {
                self.Pacing = new List<int>()
                {
                    1,
                    2,
                    3,
                    2,
                    3,
                    2,
                    1,
                    3,
                    1,
                    1,
                };
            })
            .Self(self =>
            {
                var randomIndex = UnityEngine.Random.Range(0, 2 + 1);

                switch(randomIndex)
                {
                    case 0:
                        self.InitRoom
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

                    case 1:
                        self.InitRoom
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Chest, node =>
                            {
                                node.Next(RoomTypes.Normal)
                                    .Next(RoomTypes.Normal)
                                    .Next(RoomTypes.Shop);
                            })
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Final);
                        break;

                    case 2:
                        self.InitRoom
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Normal, node =>
                            {
                                node.Next(RoomTypes.Shop)
                                    .Next(RoomTypes.Normal)
                                    .Next(RoomTypes.Chest);
                            })
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Normal)
                            .Next(RoomTypes.Final);
                        break;

                    default:
                        self.InitRoom
                            .Next(RoomTypes.Final);
                        break;
                }
                



            });
    }
}