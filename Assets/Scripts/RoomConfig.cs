using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class RoomConfig
    {   
        //房间类型
        public RoomTypes RoomType;

        //房间生成代码
        public List<string> Codes = new List<string>();

        public RoomConfig Type(RoomTypes type)
        {
            RoomType = type;
            return this;
        }

        public RoomConfig L(string code)
        {
            Codes.Add(code);
            return this;
        }
    }
}