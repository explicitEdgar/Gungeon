using System.Collections.Generic;
using static QFramework.Gungeon.RoomConfig;

namespace QFramework.Gungeon
{
    public class LevelConfig
    {
        public RoomNode InitRoom = new RoomNode(RoomTypes.Init);

        //�ؿ��ѶȽ���
        public List<int> Pacing = new List<int>();
        
        //��ͨ�ؿ�����
        public List<RoomConfig> NormalRoomTemplates { get; set; }

        public LevelConfig NormalRooms(List<RoomConfig> normalRooms)
        {
            NormalRoomTemplates = normalRooms;
            return this;
        }

        //Boss�б�
        public List<string> BossList = new List<string>();

        public LevelConfig AddBoss(string name)
        {
            BossList.Add(name);
            return this;
        }
    }
}