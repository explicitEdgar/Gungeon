using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Gungeon
{

    public enum RoomTypes
    {
        Init,
        Normal,
        Chest,
        Shop,
        Final,
    }

    public class EnemyWaveConfig
    {
        //敌人波次配置代码
        public List<string> EnemyNames = new List<string>();
    }

    //配置类
    public class Config
    {

    }

}