using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{
    public class RoomConfig
    {
        public RoomTypes RoomType;

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