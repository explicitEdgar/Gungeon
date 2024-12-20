using UnityEngine;

namespace QFramework.Gungeon
{
    public class Clip
    {
        public int clipBullet;

        public int currentClipBullet;

        public int NeedCount => clipBullet - currentClipBullet;

        public bool CanShoot => currentClipBullet > 0 && !reloading;

        public bool reloading = false;

        public bool Full => currentClipBullet == clipBullet;

        public Clip(int clipBullet)
        {
            this.clipBullet = clipBullet;
            currentClipBullet = this.clipBullet;
        }

        public void Reload(AudioClip reloadSound,int needCount = -1)
        {
            if (needCount == -1) needCount = NeedCount;
            reloading = true;
            ActionKit.Sequence()
                .PlaySound(reloadSound)
                .Callback(() =>
                {
                    reloading = false;
                    currentClipBullet += needCount;
                    UIReload();
                }).StartCurrentScene();
        }

        public void UseBullet()
        {
            this.currentClipBullet--;
            if(currentClipBullet <= 0)
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