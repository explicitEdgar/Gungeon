using UnityEngine;

namespace QFramework.Gungeon
{
    public class ShootDuration
    {
        private float mDuration;

        private float mLastShootTime;

        public bool CanShoot => Time.time - mLastShootTime >= mDuration || mLastShootTime == 0;

        public ShootDuration(float duration)
        {
            mDuration = duration;
        }

        public void RecordShootTime()
        {
            mLastShootTime = Time.time;
        }
    }
}