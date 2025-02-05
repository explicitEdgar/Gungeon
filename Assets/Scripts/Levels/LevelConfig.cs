using System.Collections.Generic;
using static QFramework.Gungeon.RoomConfig;

namespace QFramework.Gungeon
{
    public class LevelConfig
    {
        public RoomNode InitRoom = new RoomNode(RoomTypes.Init);

        //关卡难度节奏
        public List<int> Pacing = new List<int>();
        
        //普通关卡配置
        public List<RoomConfig> NormalRoomTemplates { get; set; }

        public LevelConfig NormalRooms(List<RoomConfig> normalRooms)
        {
            NormalRoomTemplates = normalRooms;
            return this;
        }

        //Boss列表
        public List<string> BossList = new List<string>();

        public LevelConfig AddBoss(string name)
        {
            BossList.Add(name);
            return this;
        }
    }
}