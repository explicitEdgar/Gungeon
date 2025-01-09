using UnityEngine;

namespace QFramework.Gungeon
{
    public class BulletBag
    {
        public int MaxBulletCount { get; set; }

        public bool hasBullet => Data.GunBagRemainBulletCount != 0;

        public bool Full => Data.GunBagRemainBulletCount == MaxBulletCount;

        public GunData Data { get; internal set; }

        public BulletBag(int MaxBulletCount)
        {
            this.MaxBulletCount = MaxBulletCount;
        }

        public void Reload(Clip clip,AudioClip reloadSound)
        {
            if(clip.Full || !hasBullet)
            {
                return;
            }
            else
            {
                var needCount = clip.NeedCount;
                if(Data.GunBagRemainBulletCount == -1)
                {
                    //子弹无限
                    //填满
                    clip.Reload(reloadSound, needCount);
                }
                else if(needCount <= Data.GunBagRemainBulletCount)
                {   
                    //填满
                    clip.Reload(reloadSound, needCount);
                    Data.GunBagRemainBulletCount -= needCount;
                    //Debug.Log("花了" + needCount);
                }
                else
                {
                    //用完剩余子弹
                    //Debug.Log("花了" + Data.GunBagRemainBulletCount);
                    clip.Reload(reloadSound, Data.GunBagRemainBulletCount);
                    Data.GunBagRemainBulletCount = 0;
                }
            }
        }
    }
}