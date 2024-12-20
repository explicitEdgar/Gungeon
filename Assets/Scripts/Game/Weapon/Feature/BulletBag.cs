using UnityEngine;

namespace QFramework.Gungeon
{
    public class BulletBag
    {
        public int RemainBulletCount { get; set; }
        public int MaxBulletCount { get; set; }

        public bool hasBullet => RemainBulletCount != 0;

        public BulletBag(int RemainBulletCount,int MaxBulletCount)
        {
            this.RemainBulletCount = RemainBulletCount;
            this.MaxBulletCount = MaxBulletCount;
        }

        public void Reload(Clip clip,AudioClip reloadSound)
        {
            if(clip.Full || !hasBullet)
            {

            }
            else
            {
                var needCount = clip.NeedCount;
                if(needCount <= RemainBulletCount)
                {   
                    //填满
                    clip.Reload(reloadSound, needCount);
                    RemainBulletCount -= needCount;
                    //Debug.Log("花了" + needCount);
                }
                else
                {
                    //用完剩余子弹
                    //Debug.Log("花了" + RemainBulletCount);
                    clip.Reload(reloadSound, RemainBulletCount);
                    RemainBulletCount = 0;
                }
            }
        }
    }
}