using UnityEngine;

namespace QFramework.Gungeon
{
    public class Clip
    {
        public int clipBullet;

        public int currentClipBullet;

        public bool CanShoot => currentClipBullet > 0 && !reloading;

        public bool reloading = false;

        public Clip(int clipBullet)
        {
            this.clipBullet = clipBullet;
            currentClipBullet = this.clipBullet;
        }

        public void Reload(AudioClip reloadSound)
        {
            reloading = true;
            ActionKit.Sequence()
                .PlaySound(reloadSound)
                .Callback(() =>
                {
                    reloading = false;
                    currentClipBullet = clipBullet;
                    UIReload();
                }).StartCurrentScene();
        }

        public void UseBullet()
        {
            this.currentClipBullet--;
            UIReload();
        }

        public void UIReload()
        {
            GameUI.Default.GunInfo.text = "弹夹容量:" + currentClipBullet + "/" + clipBullet + "(按R换弹)" ;
        }




    }
}