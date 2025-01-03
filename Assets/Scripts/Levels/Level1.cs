using System;
using UnityEngine;
namespace QFramework.Gungeon
{
    public class Level1
    {
        public static LevelConfig Config = new LevelConfig()
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
                        break;
                }
                



            });
    }
}