using System.Collections.Generic;
using static QFramework.Gungeon.RoomConfig;

namespace QFramework.Gungeon
{
    public class LevelConfig
    {
        public RoomNode InitRoom = new RoomNode(RoomTypes.Init);

        public List<int> Pacing = new List<int>();
    }
}