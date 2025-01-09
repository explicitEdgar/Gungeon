using UnityEngine;

namespace QFramework.Gungeon
{
    public class Clip
    {
        public int clipBullet;

        public int NeedCount => clipBullet - Data.CurrentBulletCount;

        public bool CanShoot => Data.CurrentBulletCount > 0 && !reloading;

        public bool reloading = false;

        public bool Full => Data.CurrentBulletCount == clipBullet;

        public GunData Data { get; internal set; }

        public Clip(int clipBullet)
        {
            this.clipBullet = clipBullet;
        }

        public void Reload(AudioClip reloadSound,int needCount = -1)
        {
            if (needCount == -1) needCount = NeedCount;
            reloading = true;
            Player.Default.gun.AudioPlayer.Stop();
            ActionKit.Sequence()
                .PlaySound(reloadSound)
                .Callback(() =>
                {
                    reloading = false;
                    Data.CurrentBulletCount += needCount;
                    UIReload();
                }).StartCurrentScene();
        }

        public void UseBullet()
        {
            this.Data.CurrentBulletCount--;
            if(Data.CurrentBulletCount <= 0)
            {
                Player.DisplayText("我没有子弹了", 2f);
            }
            UIReload();
        }

        public void UIReload()
        {
            GameUI.UpdateGunInfo(this);
        }




    }
}